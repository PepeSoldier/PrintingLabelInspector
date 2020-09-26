namespace MDL_iLOGIS.ComponentWMS.Enums
{
    public enum EnumMovementType
    {
        //In = 10,
        //Out = 20,
        //Shift = 30,
        //Consumption = 40,
        //Inventory = 50,
        //Scrap = 60
        Unassigned = 0,
        ///<summary>Goods receipt for purchase order</summary>
        CODE_101 = 101,
        ///<summary>Goods receipt for purchase order to GR blocked stock</summary>
        CODE_102 = 102,
        ///<summary>Goods receipt UNDO???????</summary>
        CODE_103 = 103,
        ///<summary>Release from the GR blocked stock for the purchase order</summary>
        CODE_105 = 105,
        ///<summary>Subsequent adjustment for subcontracting</summary>
        CODE_121 = 121,
        ///<summary>Return deliveries to vendor</summary>
        CODE_122 = 122,
        ///<summary>Return delivery to vendor from GR blocked stock</summary>
        CODE_124 = 124,
        ///<summary>Returns for purchase order</summary>
        CODE_161 = 161,
        ///<summary>Goods issue for a cost center</summary>
        CODE_201 = 201,
        ///<summary>Goods issue for a project</summary>
        CODE_221 = 221,
        ///<summary>Goods issue for sale (without sales order)</summary>
        CODE_251 = 251,
        ///<summary>Goods issue for an order</summary>
        CODE_261 = 261,
        ///<summary>Goods issue for a network</summary>
        CODE_281 = 281,
        ///<summary>Goods issue for any account assignment</summary>
        CODE_291 = 291,
        ///<summary>Plant to plant transfer in one step</summary>
        CODE_301 = 301,
        ///<summary>Plant to plant transfer in two steps : stock removal</summary>
        CODE_303 = 303,
        ///<summary>Plant to plant transfer in two steps : putaway</summary>
        CODE_305 = 305,
        ///<summary>Transfer postings from material to material</summary>
        CODE_309 = 309,
        ///<summary>STORNO: Transfer postings from material to material</summary>
        CODE_310 = 310,
        ///<summary>Transfer of storage location to storage location in one step</summary>
        CODE_311 = 311,
        ///<summary>Storno?</summary>
        CODE_312 = 312,
        ///<summary>Transfer of storage location to storage location in two steps : stock removal</summary>
        CODE_313 = 313,
        ///<summary>Transfer of storage location to storage location in two steps : putaway</summary>
        CODE_315 = 315,
        ///<summary>Transfer of inspection stock : unrestricted-use stock</summary>
        CODE_321 = 321,
        ///<summary>Transfer of storage location to storage location : inspection stock</summary>
        CODE_323 = 323,
        ///<summary>Transfer of storage location to storage location : blocked stock</summary>
        CODE_325 = 325,
        ///<summary>Sample from the inspection stock</summary>
        CODE_331 = 331,
        ///<summary>Sample from the unrestricted-use stock</summary>
        CODE_333 = 333,
        ///<summary>Sample from the blocked stock</summary>
        CODE_335 = 335,
        ///<summary>Status change of a batch (unrestricted-use to restricted)</summary>
        CODE_341 = 341,
        ///<summary>Transfer of blocked stock : unrestricted-use stock</summary>
        CODE_343 = 343,
        ///<summary>Transfer of blocked stock : inspection stock</summary>
        CODE_349 = 349,
        ///<summary>Goods issue for a stock transport order (without shipping)</summary>
        CODE_351 = 351,
        ///<summary>Transfer of special stock to own stock (only for sales order stock)</summary>
        CODE_411 = 411,
        ///<summary>Transfer posting to sales order stock</summary>
        CODE_413 = 413,
        ///<summary>Returns from customer (without shipping)</summary>
        CODE_451 = 451,
        ///<summary>Transfer of blocked stock returns to unrestricted-use stock</summary>
        CODE_453 = 453,
        ///<summary>Returns stock transfer</summary>
        CODE_455 = 455,
        ///<summary>Transfer of blocked stock returns to inspection stock</summary>
        CODE_457 = 457,
        ///<summary>Transfer of blocked stock returns to blocked stock</summary>
        CODE_459 = 459,
        ///<summary>Goods receipt without purchase order : unrestricted-use stock</summary>
        CODE_501 = 501,
        ///<summary>Goods receipt without purchase order : stock in quality inspection</summary>
        CODE_503 = 503,
        ///<summary>Goods receipt without purchase order : blocked stock</summary>
        CODE_505 = 505,
        ///<summary>Goods receipt without order : unrestricted-use stock</summary>
        CODE_521 = 521,
        ///<summary>Goods receipt without order : inspection stock</summary>
        CODE_523 = 523,
        ///<summary>Goods receipt without order : blocked stock</summary>
        CODE_525 = 525,
        ///<summary>Goods receipt of by-products from order</summary>
        CODE_531 = 531,
        ///<summary>Transfer of unrestricted-use stock to subcontracting stock</summary>
        CODE_541 = 541,
        ///<summary>Consumption from subcontracting stock</summary>
        CODE_543 = 543,
        ///<summary>Goods receipt of by-products from subcontracting</summary>
        CODE_545 = 545,
        ///<summary>Scrapping from unrestricted-use stock</summary>
        CODE_551 = 551,
        ///<summary>Scrapping from inspection stock</summary>
        CODE_553 = 553,
        ///<summary>Scrapping from blocked stock</summary>
        CODE_555 = 555,
        ///<summary>Issue from stock in transit (adjustment posting)</summary>
        CODE_557 = 557,
        ///<summary>Initial entry of stock balances : unrestricted-use stock</summary>
        CODE_561 = 561,
        ///<summary>Initial entry of stock balances : quality inspection</summary>
        CODE_562 = 562,
        ///<summary>Initial entry of stock balances : quality inspection</summary>
        CODE_563 = 563,
        ///<summary>Initial entry of stock balances : blocked stock</summary>
        CODE_565 = 565,
        ///<summary>Goods receipt of a by-product from network</summary>
        CODE_581 = 581,
        ///<summary>Goods issue for delivery</summary>
        CODE_601 = 601,
        ///<summary>Goods issue for a stock transport order (shipping) with additional item</summary>
        CODE_603 = 603,
        ///<summary>Goods receipt for a stock transport order (shipping) with additional item</summary>
        CODE_605 = 605,
        ///<summary>Transfer of unrestricted-use stock : returnable packaging with customer (shipping)</summary>
        CODE_621 = 621,
        ///<summary>Goods issue from returnable packaging with customer (shipping)</summary>
        CODE_623 = 623,
        ///<summary>Transfer of unrestricted-use stock : consignment stock at customer (shipping)</summary>
        CODE_631 = 631,
        ///<summary>Goods issue from consignment stock at customer (shipping)</summary>
        CODE_633 = 633,
        ///<summary>Goods issue for a stock transport order (shipping)</summary>
        CODE_641 = 641,
        ///<summary>Goods issue for a cross-company-code stock transport order</summary>
        CODE_643 = 643,
        ///<summary>Goods issue for a cross-company-code stock transport order performed in one step (shipping)</summary>
        CODE_645 = 645,
        ///<summary>Goods issue for a stock transport order performed in one step (shipping)</summary>
        CODE_647 = 647,
        ///<summary>Returns from customer (shipping)</summary>
        CODE_651 = 651,
        ///<summary>Returns from customer (shipping) to unrestricted-use stock</summary>
        CODE_653 = 653,
        ///<summary>Returns from customer (shipping) to inspection stock</summary>
        CODE_655 = 655,
        ///<summary>Returns from customer (shipping) to blocked stock</summary>
        CODE_657 = 657,
        ///<summary>Returns to vendor using shipping</summary>
        CODE_661 = 661,
        ///<summary>Returns for a cross-company-code stock transport order (shipping)</summary>
        CODE_673 = 673,
        ///<summary>Returns for a cross-company-code stock transport order (shipping) performed in one step</summary>
        CODE_675 = 675,
        ///<summary>Inventory difference in unrestricted-use stock</summary>
        CODE_701 = 701,
        ///<summary>Inventory difference in quality inspection stock (MM-IM)</summary>
        CODE_703 = 703,
        ///<summary>Inventory difference in blocked stock</summary>
        CODE_707 = 707,
        ///<summary>Inventory difference in unrestricted-use stock (LE-WM)</summary>
        CODE_711 = 711,
        ///<summary>STORNO: Inventory difference in unrestricted-use stock (LE-WM)</summary>
        CODE_712 = 712,
        ///<summary>Inventory difference in quality inspection stock (MM-IM)</summary>
        CODE_713 = 713,
        ///<summary>Inventory difference for returns</summary>
        CODE_715 = 715,
        ///<summary>Inventory difference in blocked stock (LE-WM)</summary>
        CODE_717 = 717,


    }
}