using System.Web;
using System.Web.Mvc;
using GridMvc.Columns;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GridMvc
{
    public class GridCellRenderer : GridStyledRenderer, IGridCellRenderer
    {
        private const string TdClass = "grid-cell";

        public GridCellRenderer()
        {
            AddCssClass(TdClass);
        }

        public IHtmlString Render(IGridColumn column, IGridCell cell)
        {
            return Render(column, cell, false);
        }

        public IHtmlString Render(IGridColumn column, IGridCell cell, bool isFake)
        {
            string cssStyles = GetCssStylesString();
            string cssClass = GetCssClassesString();

            var builder = new TagBuilder("td");
            if (!string.IsNullOrWhiteSpace(cssClass))
                builder.AddCssClass(cssClass);
            if (!string.IsNullOrWhiteSpace(cssStyles))
                builder.MergeAttribute("style", cssStyles);

            string value = isFake ? "" : cell.ToString();

            if (column.ShowManageBtncolumn)
            {
                string buttons = "";
                if (isFake && column.SaveActionUrl != "" && column.CanInsert)
                {
                    buttons += RenderButtonCreate();
                    buttons += RenderButtonCreateXls();
                }

                buttons += RenderButtonEdit(isFake);
                buttons += RenderButtonCancel(value);

                if (column.OpenActionUrl != "")
                    RenderButtonOpen(column.OpenActionUrl + "/" + value);
                if (column.SaveActionUrl != "")
                    buttons += RenderButtonSave(column.SaveActionUrl, value);
                if (column.DeleteActionUrl != "")
                    buttons += RenderButtonDelete(column.DeleteActionUrl + "/" + value);

                buttons += " " + RenderLabelStatus();

                builder.InnerHtml = buttons;
            }
            else if (column.EditableColumn)
            {
                builder.MergeAttribute("data-editable", "true");
                builder.MergeAttribute("data-value", value);
                builder.MergeAttribute("data-name", column.Name.Replace(".Name", "Id").Replace(".Code", "Id"));
                builder.MergeAttribute("data-type", column.EditableColumnType);
                builder.MergeAttribute("data-list-items", column.EditableColumnItems);

                if (!isFake && column.EditableColumnType == "select")
                {
                    Dictionary<string, string> listItems = JsonConvert.DeserializeObject<Dictionary<string, string>>(column.EditableColumnItems);
                    string tmp;
                    listItems.TryGetValue(cell.ToString(), out tmp);
                    builder.InnerHtml = RenderCell(tmp);
                } else if (!isFake && column.EditableColumnType == "checkbox")
                {
                    string tmp = (value == "True" ? "<i class=\"fa fa-check\"></i>" : "<i class=\"fa fa-times\"></i>");
                    builder.InnerHtml = RenderCell(tmp);
                }
                else
                {
                    builder.InnerHtml = RenderCellEditable(cell.ToString(), column.Name); //RenderCell(value);
                }
            }
            else
            {
                builder.InnerHtml = value;
            }

            return MvcHtmlString.Create(builder.ToString());
        }

        private string RenderCell(string value)
        {
            var label = new TagBuilder("label");
            label.InnerHtml = value;
            label.AddCssClass("display-mode");
            return label.ToString();
        }
        private string RenderCellEditable(string value, string name)
        {
            var builder2 = new TagBuilder("span");

            var lb = new TagBuilder("label");
            lb.InnerHtml = value;
            lb.AddCssClass("display-mode");
            lb.MergeAttribute("for", name);

            var tb = new TagBuilder("input");
            tb.AddCssClass("form-control edit-mode");
            tb.MergeAttribute("type", "text");
            tb.MergeAttribute("name", name);
            tb.MergeAttribute("id", name);
            tb.MergeAttribute("value", value);

            // System.Web.Mvc.Html.SelectExtensions.DropDownList()

            builder2.InnerHtml = lb.ToString() + tb.ToString();
            return builder2.ToString();
        }

        private string RenderButtonCreate()
        {
            var btn = new TagBuilder("button");
            btn.InnerHtml = "<span class=\"glyphicon glyphicon-plus\"></span> <span>Dodaj</span>";
            btn.AddCssClass("create-btn display-mode btn btn-default");
            return btn.ToString();
        }

        private string RenderButtonCreateXls()
        {
            var btn = new TagBuilder("button");
            btn.InnerHtml = "<span class=\"glyphicon glyphicon-list-alt\"></span> <span>Dodaj z XLS</span>";
            btn.AddCssClass("create-xls-btn display-mode btn btn-default");
            return btn.ToString();
        }

        private string RenderButtonOpen(string href)
        {
            var btn = new TagBuilder("a");
            btn.InnerHtml = "Otwórz";
            btn.AddCssClass("edit-btn display-mode btn btn-default");
            btn.MergeAttribute("href", href);
            return btn.ToString();
        }
        private string RenderButtonEdit(bool hidden)
        {
            var btn = new TagBuilder("div");
            btn.InnerHtml = "<span class=\"glyphicon glyphicon-edit\"></span> <span>Edytuj</span>";
            btn.AddCssClass("edit-btn display-mode btn btn-default");
            if (hidden)
            {
                btn.MergeAttribute("style", "display: none");
            }
            return btn.ToString();
        }
        private string RenderButtonSave(string href, string id)
        {
            var btn = new TagBuilder("button");
            btn.InnerHtml = "<span class=\"glyphicon glyphicon-floppy-disk\"></span> <span></span>";
            btn.AddCssClass("save-btn edit-mode btn btn-success");
            btn.MergeAttribute("data-toggle", "tooltip");
            btn.MergeAttribute("title", "Zapisz");
            btn.MergeAttribute("data-href", href);
            btn.MergeAttribute("data-id", id);
            btn.MergeAttribute("style", "display: none");
            return btn.ToString();
        }
        private string RenderButtonCancel(string id)
        {
            var btn = new TagBuilder("button");
            btn.InnerHtml = "<span class=\"glyphicon glyphicon-remove\"></span>";
            btn.AddCssClass("cancel-btn edit-mode btn btn-warning");
            btn.MergeAttribute("data-toggle", "tooltip");
            btn.MergeAttribute("title", "Anuluj");
            btn.MergeAttribute("data-id", id);
            btn.MergeAttribute("style", "display: none");
            return btn.ToString();
        }
        private string RenderButtonDelete(string href)
        {
            var btn = new TagBuilder("button");
            btn.InnerHtml = "<span class=\"glyphicon glyphicon-trash\"></span>";
            btn.AddCssClass("delete-btn edit-mode btn btn-danger");
            btn.MergeAttribute("data-toggle", "tooltip");
            btn.MergeAttribute("title", "Usuń");
            btn.MergeAttribute("data-href", href);
            btn.MergeAttribute("style", "display: none");
            return btn.ToString();
        }

        private string RenderLabelStatus()
        {
            var label = new TagBuilder("label");
            label.AddCssClass("edit-mode status");
            label.MergeAttribute("style", "display: none");
            return label.ToString();
        }



    }
}