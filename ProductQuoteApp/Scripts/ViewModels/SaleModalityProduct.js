/// <reference path="../jquery-3.2.1.min.js" />
/// <reference path="../jquery-ui-1.12.1.min.js" />
/// <reference path="../knockout-3.4.2.js" />
/// <reference path="../knockout.validation.min.js" />

(function () {

    ko.validation.init({
        insertMessages: true,
        messagesOnModified: true,
        decorateElement: true,
        errorElementClass: 'wrong-field'
    }, true);

    var MSViewModel = function () {

        var self = this;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ko.extenders.numeric = function (target, precision) {
            //create a writable computed observable to intercept writes to our observable
            var result = ko.pureComputed({
                read: target,  //always return the original observables value
                write: function (newValue) {
                    var current = target(),
                        roundingMultiplier = Math.pow(10, precision),
                        newValueAsNum = isNaN(newValue) ? 0 : +newValue,
                        valueToWrite = Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier;

                    //only write if it changed
                    if (valueToWrite !== current) {
                        target(valueToWrite);
                    } else {
                        //if the rounded value is the same, but a different value was written, force a notification for the current field
                        if (newValue !== current) {
                            target.notifySubscribers(valueToWrite);
                        }
                    }
                }
            }).extend({ notify: 'always' });

            //initialize with current value to make sure it is rounded appropriately
            result(target());

            //return the new computed observable
            return result;
        };
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        self.SaleModalityID = ko.observable();
        self.SaleModalitys = ko.observableArray([]);
        self.Products = ko.observableArray([]);
        self.ErrorMessage = ko.observable();
        self.Description = ko.observable(); 

        self.productList = ko.observableArray([]);
        self.selectedProductID = ko.observable().extend({
            required: {
                params: true,
                message: "Debe seleccionar un valor de la lista."
            }
        });

        self.masterGridSelected = ko.observable(false);

        self.productCostAdd = ko.observable().extend({
            required: {
                params: true,
                message: "Debe ingresar un valor."
            }
        });

        self.template = ko.observable("TemplateA");

        self.toggle = function () {
            self.template(self.template() === "TemplateA" ? "TemplateB" : "TemplateA");
        };

        var _itemTemplate;

        self.templateSelector = function (viewModel) {
            if (!_itemTemplate) {
                _itemTemplate = ko.computed(function () { return this.template(); }, viewModel);
            }

            return _itemTemplate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // los paneles de edicion y agregado no se muestran cuando arranca
        self.showAddProduct = ko.observable(false);
        self.showEditProduct = ko.observable(false);

        self.isSelected = function (data) {
            if (data.SaleModalityID === self.SaleModalityID()){
                self.masterGridSelected(true);
                return "selected";
            }
        };

        //The SaleModality
        var SaleModality = {
            SaleModalityID: self.SaleModalityID,
            Description: self.Description
        }

        //The SaleModalityProduct
        var SaleModalityProduct = {
            SaleModalityID: self.SaleModalityID,
            ProductID: self.selectedProductID,
            ProductCost: self.productCostAdd
        }

        loadSaleModalitys();

        //Llamado API para cargar las Modalidades de Venta
        function loadSaleModalitys() {
            $.ajax({
                url: "GetSaleModalitysMM2",
                type: "GET",
                success: function (resp) {
                    self.SaleModalitys(resp);
                },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        };

        //Se muestran los Costos por Producto por Modalidad de Venta seleccionada.
        self.openDetailsDialog = function (saleMod) {
            self.SaleModalityID(saleMod.SaleModalityID);
            self.Description(saleMod.Description); //USed to Display SaleModality Name
            getProductBySaleModality(saleMod);
            self.showEditProduct(false);
            self.showAddProduct(false);
            self.template("TemplateA");
        }

        //Llamado API para cargar de los Productos con sus Costos por Modalidad de venta seleccionada.
        function getProductBySaleModality(saleMod) {
            $.ajax({
                url: "GetProductBySaleModality/" + saleMod.SaleModalityID,
                type: "GET",
                success: function (resp) { self.Products(resp); },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        }

        //Llamado API para cargar de los Productos con sus Costos por Modalidad de venta seleccionada (por Id).
        function getProductBySaleModalityById(saleModalityID) {
            $.ajax({
                url: "GetProductBySaleModality/" + saleModalityID,
                type: "GET",
                success: function (resp) { self.Products(resp); },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        }

        //Llamado API para cargar el combo de Productos
        self.loadProductAvailables = function (saleModalityID) {
            if (saleModalityID !== undefined) {
                $.ajax({
                    url: "GetProductAvailables/" + saleModalityID,
                    type: "GET",
                    success: function (resp) { self.productList(resp); },
                    error: function (err) { alert("Ha ocurrido un error, por favor intente mas tarde" + err.status); }
                });
            }
            else {
                //self.saleModalityList.removeAll();
                //self.selectedProductID(undefined);
            }
        }

        //Boton AGREGAR PRODUCTO
        self.addProduct = function () {
            self.showEditProduct(false);
            self.showAddProduct(true);
            self.loadProductAvailables(self.SaleModalityID());
        }

        //Boton GRABAR NUEVO
        self.saveAddProduct = function () {

            //debug
            console.log(ko.toJSON(self.Products()));

            if (self.errors().length > 0) {
                self.errors.showAllMessages();
                return;
            }
            $.ajax({
                url: "CreateSaleModalityProduct",
                type: "POST",
                data: ko.toJSON(SaleModalityProduct),
                contentType: "application/json",
                success: function (SaleModalityProduct) {
                    getProductBySaleModalityById(SaleModalityProduct.SaleModalityID);
                },
                error: function () {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                }
            });

            self.showEditProduct(false);
            self.showAddProduct(false);
        }

        //Boton CANCELAR ADD
        self.cancelAddProduct = function (product) {
            self.showEditProduct(false);
            self.showAddProduct(false);
        }

        //Boton EDITAR
        self.editProduct = function () {
            self.toggle(self);
            self.showEditProduct(true);
            self.showAddProduct(false);
        }

        //Boton GRABAR EDICION
        self.saveEditProduct = function () {
            
            //debug
            console.log(ko.toJSON(self.Products()));
            //alert(ko.toJSON(self.Products()));

            $.ajax({
                url: "UpdateSaleModalityProductList",
                type: "POST",
                data: ko.toJSON(self.Products()),
                contentType: "application/json",
                success: function () {
                    getProductBySaleModalityById(self.SaleModalityID());
                },
                error: function () {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                }
            });

            self.toggle(self);
            self.showEditProduct(false);
            self.showAddProduct(false);
        }

        //Boton CANCELAR EDICION
        self.cancelEditProduct = function (product) {
            self.toggle(self);
            self.showEditProduct(false);
            self.showAddProduct(false);
        }

        self.deleteProduct = function (product) {
            $.ajax({
                url: "api/SaleModalityProductApi/" + product.SaleModalityProductID,
                type: "DELETE",
                success: function (data) {
                    getProductBySaleModalityById(product.SaleModalityID);
                },
                error: function (error) {
                    alert(error.status + "<--and--> " + error.statusText);
                }
            });
        }

    };


    var vm = new MSViewModel();
    vm.errors = ko.validation.group(vm);

    ko.applyBindings(vm);

    //ko.applyBindings(new MSViewModel());

})();
