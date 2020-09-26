class iLogisWmsMobileBackButton extends HTMLElement {

    constructor() {
        super();
        this.Actions();
    }
    connectedCallback() {   
        console.log("niby render a jednak nei render");
        this.Render(this);
    }
    disconnectedCallback() { }
    static get observedAttributes() { return [/* ... */]; }
    attributeChangedCallback(name, oldValue, newValue) { /* ... */ }
    adoptedCallback() { }

    Render(targetElement) {
        fetch('/Areas/iLOGIS/Views/WMS/BackButtonComponent.cshtml')
          .then((response) => response.text())
          .then((template) => {
              var rendered = Mustache.render(template);
              $(targetElement).html(rendered);
          });
    }

    Actions() {
        $(document).on("click", "#testbtn", function () {
            console.log("testbtn clicked 2");
        });
    }
}
customElements.define('ilogis-wms-mobile-backbutton', iLogisWmsMobileBackButton);
