namespace MDLX_MASTERDATA.Enums
{
    public enum ItemTypeEnum
    {
        Undefined = 0,
        RawMaterial = 10,       //surowiec
        BuyedItem = 20,         //pozycja zakupowa
        IntermediateItem = 30,  //pozycja pośrednia
        FinishedItem = 40,      //półfabrykat
        Product = 50,           //PNC-Wyrób gotowy (np. zmywarka)
        ItemGroup = 90
    }
}