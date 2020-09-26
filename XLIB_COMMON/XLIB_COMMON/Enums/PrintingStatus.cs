using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XLIB_COMMON.Enums
{
    public enum PrintingStatus
    {
        NotDefinedPrinterType = 1,
        ReadyToConnect = 5,
        ProblemWithFileRead = 6,
        Printed = 7,
        ProblemWithPreparingLabel = 8,
        ProblemWithParsingDataToLabel = 9,
        ErrorGettingStatusFromPrinter = 20,
        UnableToGetStatus = 21,
        PrinterIsPaused = 22,
        HeadIsOpen = 23,
        CannotPrint = 24,
        ReadyToPrint = 25,
        UnrecognizedProblem = 30
    }

}