using System;
using UnityEngine;
using UnityEngine.Purchasing; //���������� � ���������, ����� �������� ����� ���������� �������

public class PurchaseController : MonoBehaviour, IStoreListener //��� ��������� ��������� �� Unity Purchasing
{
    [SerializeField] private GameObject panelAds;
    [SerializeField] private GameObject panelVIP;

    [SerializeField] private GameObject panelAdsDone;
    [SerializeField] private GameObject panelVIPDone;

    private static IStoreController m_StoreController;          //������ � ������� Unity Purchasing
    private static IExtensionProvider m_StoreExtensionProvider; // ���������� ������� ��� ���������� ���������

    public static string noads = "noads"; //����������� - nonconsumable
    public static string vip = "vip"; //����������� - nonconsumable ��� ����� ���� ��������
    public static string coins1000 = "coins1000"; //������������ - consumable

    void Start()
    {
        if (m_StoreController == null) //���� ��� �� ���������������� ������� Unity Purchasing, ����� ��������������
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized()) //���� ��� ���������� � ������� - ������� �� �������
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //����������� ���� ������ ��� ���������� � ������
        builder.AddProduct(noads, ProductType.NonConsumable);
        builder.AddProduct(vip, ProductType.NonConsumable); //��� ProductType.Subscription
        builder.AddProduct(coins1000, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyNoAds()
    {
        BuyProductID(noads);
    }

    public void BuyVip()
    {
        BuyProductID(vip);
    }

    public void BuyCoins1000()
    {
        BuyProductID(coins1000);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized()) //���� ������� ���������������� 
        {
            Product product = m_StoreController.products.WithID(productId); //������� ������� ������� 

            if (product != null && product.availableToPurchase) //���� ������� ������ � ����� ��� �������
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product); //��������
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) //�������� �������
    {
        if (String.Equals(args.purchasedProduct.definition.id, noads, StringComparison.Ordinal)) //��� �������� ��� ID
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            //�������� ��� �������
            if (PlayerPrefs.HasKey("ads") == false)
            {
                PlayerPrefs.SetInt("ads", 0);
                panelAds.SetActive(false);
                panelAdsDone.SetActive(true);

                /*AdsCore.S.HideBanner();
                AdsCore.S.StopAllCoroutines();*/
            }

        }
        else if (String.Equals(args.purchasedProduct.definition.id, vip, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            //�������� ��� �������
            if (PlayerPrefs.HasKey("vip") == false)
            {
                PlayerPrefs.SetInt("vip", 0);
                panelVIP.SetActive(false);
                panelVIPDone.SetActive(true);
            }
        }
        else if (String.Equals(args.purchasedProduct.definition.id, coins1000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            //�������� ��� �������
            /*GameLogic.S.IncrementPoint2AfterAds(1000);*/
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    public void RestorePurchases() //�������������� ������� (������ ��� Apple). � ���� ��� �������������� �������.
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) //���� ��������� �� ��� ����������
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("ads") == true)
        {
            panelAds.SetActive(!PlayerPrefs.HasKey("ads"));
            panelAdsDone.SetActive(PlayerPrefs.HasKey("ads"));
        }

        if (PlayerPrefs.HasKey("vip") == true)
        {
            panelVIP.SetActive(!PlayerPrefs.HasKey("vip"));
            panelVIPDone.SetActive(PlayerPrefs.HasKey("vip"));
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}