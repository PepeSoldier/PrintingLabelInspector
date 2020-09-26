using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDLX_CORE.Model.PrintModels
{
    public abstract class PrintLabelModelAbstract
    {
        public PrintLabelModelAbstract(string printerIp)
        {
            PrinterIP = printerIp;
        }

        protected LabelLayouts LabelLayout;
        protected PrintingStatus labelStatus;
        protected string labelFullText;
        protected string PrinterIP;
        protected string fileExtension;
        public string FileExtension
        {
            get
            {
                return fileExtension;
            }
        }

        public PrintingStatus Print()
        {
            if (labelFullText != null && labelFullText.Length > 2)
            {
                labelStatus = SendLabelToPrinter();
            }
            else
            {
                labelStatus = PrintingStatus.ProblemWithParsingDataToLabel;
            }
            return labelStatus;  
        }
        public virtual void PrepareLabelFromLayout(int layoutNo, LabelData labelData)
        {
            LabelLayout = new LabelLayouts(labelData);
            labelFullText = LabelLayout.GetLayout(layoutNo);
        }
        public virtual void PrepareLabel(string rawText, LabelData labelData)
        {
            labelFullText = ParseStringWithModel(rawText, labelData);
        }
        
        protected abstract PrintingStatus SendLabelToPrinter();

        private string ParseStringWithModel(string labelDefinitionText, LabelData labelData)
        {
            if (labelDefinitionText == null) return null;

            string labelText = labelDefinitionText;
            int nextOpeningMustacheIndex = labelText.IndexOf("{");
            int nextClosingMustacheIndex = labelText.IndexOf("}");
            string textInMustaches;
            string propertName;
            object propertyValue;

            while (nextOpeningMustacheIndex != -1)
            {
                textInMustaches = labelText.Substring(nextOpeningMustacheIndex, (nextClosingMustacheIndex - nextOpeningMustacheIndex) + 2);
                propertName = textInMustaches.Replace('{', ' ').Replace('}', ' ').Trim();
                propertyValue = GetPropValue(labelData, propertName);
                labelText = labelText.Replace(textInMustaches, (string)propertyValue);

                nextOpeningMustacheIndex = labelText.IndexOf("{");
                nextClosingMustacheIndex = labelText.IndexOf("}");
            }
            return labelText;
        }
        private object GetPropValue(object src, string propName)
        {
            try
            {
                return src.GetType().GetProperty(propName).GetValue(src, null);
            }
            catch
            {
                return "??";
            }
        }
    }
}