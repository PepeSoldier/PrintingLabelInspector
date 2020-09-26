class MyHeader extends HTMLElement {

    constructor() {
        super();
        this.Actions();
    }
    connectedCallback() {
        var model = { headerTitle: $(this).attr("headerTitle"), icon: $(this).attr("icon") };
        this.Render(this, model);
    }
    disconnectedCallback() { }
    static get observedAttributes() {
        return [/* ... */];
    }
    attributeChangedCallback(name, oldValue, newValue) {
    };
    adoptedCallback() { }

    Render(targetElement, data) {
        fetch('/Areas/iLOGIS/Views/WMS/PageHeaderComponent.cshtml')
          .then((response) => response.text())
          .then((template) => {
              var rendered = Mustache.render(template, data);
              $(targetElement).html(rendered);
          });
    }

    Actions() {}
}
customElements.define('ilogis-wms-mobile-pageheader', MyHeader);
