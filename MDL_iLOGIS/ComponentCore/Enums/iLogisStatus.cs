using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentCore.Enums
{
    public enum iLogisStatus
    {
        NoError = 0,
        GeneralError = 1,

        LocationNotFound = 100,
        LocationIsNotEmpty = 110,
        LocationIsFull = 120,
        LocationUtilizationInsufficient = 125,
        LocationTypeIncompatibility = 130,
        LocationNotFoundForSomeItems = 140,
        LocationNotTheSame = 150,

        StockUnitNotFound = 200,
        StockUnitCreated = 201,
        StockUnitDeleted = 202,
        StockUnitUpdated = 203,
        StockUnitAlreadyExists = 205,
        StockUnitQtyInPackageIsLess = 210,
        StockUnitPutQtyGreeaterThanZero = 212,
        StockUnitUoMConversionProblem = 213,
        StockUnitUoMIncorrect = 214,
        StockUnitDeletionProblem = 220,
        StockUnitRequestedQtyNotAvailable = 230,
        ItemWMSNotFound =  300,
        ItemWMSNotTheSame =  310,

        PackageItemNotFound = 400,

        SupplierNotFound = 500,

        DeliveryNotFound = 600,
        DeliverySupplierNotCorrect = 610,

        WorkstationNotFoud = 700,
        WarehouseNotFoud = 730,
        WarehouseSourceNotFoud = 731,
        WarehouseDestNotFoud = 732,

        PrintingProblem = 900,
        MovementDoneButLabelsWereNotPrinted = 901,
        LabelPrinted = 902,
        LabelCantBePrinted = 903,

        WHDocPDFCreatedSuccessfull = 1000,
        WHDocPDFCreatingFailed = 1001,
        WHDocNotSignedYet= 1005,
        WHDocCantEditAlreadySigned = 1010,

    }
}
