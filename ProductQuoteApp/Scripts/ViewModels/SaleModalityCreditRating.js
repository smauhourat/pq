/// <reference path="../jquery-3.2.1.min.js" />
/// <reference path="../jquery-ui-1.12.1.min.js" />
/// <reference path="../knockout-3.4.2.js" />
/// <reference path="../knockout.validation.min.js" />

(function () {

    ko.validation.init({
        insertMessages: true,
        messagesOnModified: true,
        decorateElement: true,
        errorElementClass: 'wrong-field',
        errorClass: 'wrong-field'
    }, true);

    var validateFloatPositive = function (val) {
        if ((val === undefined) || val === "")
            return true;

        val = (val + "").replace(",", "").replace(".", "")
        val = parseFloat(val);
        if ((!Number.isInteger(val)) || val < 0)
            return false;
        else
            return true;
    };

    var validateFloatPositivePercentage = function (val) {
        if ((val === undefined) || val === "")
            return true;

        val = (val + "").replace(",", ".");
        val = parseFloat(val);
        
        if ((!Number(val)) || val < 0)
            return false;
        else
        {
            if (val > 0 && val <= 100)
                return true;
            else
                return false;
        }
            
    };

    var SaleModalityCreditRating = function (saleModalityCreditRatingID, saleModalityID, saleModalityDescription, creditRatingID, creditRatingDescription, minimumMarginPercentage, minimumMarginUSD) {
    
        var self = this;
        self.saleModalityCreditRatingID = ko.observable(saleModalityCreditRatingID);
        self.saleModalityID = ko.observable(saleModalityID);
        self.saleModalityDescription = ko.observable(saleModalityDescription);
        self.creditRatingID = ko.observable(creditRatingID);
        self.creditRatingDescription = ko.observable(creditRatingDescription);
        self.minimumMarginPercentage = ko.observable(minimumMarginPercentage);
        self.minimumMarginUSD = ko.observable(minimumMarginUSD);

        return self;
    };


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
        self.CreditRatings = ko.observableArray([]);
        self.ErrorMessage = ko.observable();
        self.Description = ko.observable();


        self.creditRatingList = ko.observableArray([]);
        self.selectedCreditRatingID = ko.observable().extend({
            required: {
                params: true,
                message: "Debe seleccionar un valor de la lista."
            }
        });

        self.masterGridSelected = ko.observable(false);

        self.minimumMarginPercentageAdd = ko.observable().extend({
            required: {
                params: true,
                message: "Debe ingresar un valor."
            },
            validation: {
                validator: validateFloatPositivePercentage,
                message: 'Debe ser un número entero mayor o igual a cero y menor a 100'
            }
        });

        self.minimumMarginUSDAdd = ko.observable().extend({
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

        self.FillCreditRatings = function (arg) {

            self.CreditRatings([]);
            for (var i = 0; i < arg.length; i++) {
                self.CreditRatings.push(new SaleModalityCreditRating(arg[i].SaleModalityCreditRatingID, arg[i].SaleModalityID, arg[i].SaleModalityDescription, arg[i].CreditRatingID, arg[i].CreditRatingDescription, arg[i].MinimumMarginPercentage, arg[i].MinimumMarginUSD));
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // los paneles de edicion y agregado no se muestran cuando arranca
        self.showAddCreditRating = ko.observable(false);
        self.showEditCreditRating = ko.observable(false);

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

        //The SaleModalityCreditRating
        var SaleModalityCreditRatingToAdd = {
            SaleModalityID: self.SaleModalityID,
            CreditRatingID: self.selectedCreditRatingID,
            MinimumMarginPercentage: self.minimumMarginPercentageAdd,
            MinimumMarginUSD: self.minimumMarginUSDAdd
        }

        loadSaleModalitys();

        //Llamado API para cargar las Modalidades de Venta
        function loadSaleModalitys() {
            $.ajax({
                url: "GetSaleModalitysMM",
                type: "GET",
                success: function (resp) {
                    self.SaleModalitys(resp);
                },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        };

        //Se muestran los Margenes por Clasificacion Crediticia por Modalidad de Venta seleccionada.
        self.openDetailsDialog = function (saleMod) {
            self.SaleModalityID(saleMod.SaleModalityID);
            self.Description(saleMod.Description); //USed to Display SaleModality Name
            getCreditRatingBySaleModality(saleMod);
            self.showEditCreditRating(false);
            self.showAddCreditRating(false);
            self.template("TemplateA");
        }

        //Llamado API para cargar de las Clasificaciones Crediticias con sus Margenes por Modalidad de venta seleccionada.
        function getCreditRatingBySaleModality(saleMod) {
            $.ajax({
                url: "GetCreditRatingBySaleModality/" + saleMod.SaleModalityID,
                type: "GET",
                success: function (resp) {
                    //self.CreditRatings(resp);
                    //console.log(ko.toJSON(resp));
                    self.FillCreditRatings(resp);
                },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        }

        //Llamado API para cargar de las Clasificaciones Crediticias con sus Margenes por Modalidad de venta seleccionada (por Id).
        function getCreditRatingBySaleModalityById(saleModalityID) {
            $.ajax({
                url: "GetCreditRatingBySaleModality/" + saleModalityID,
                type: "GET",
                success: function (resp) {
                    //self.CreditRatings(resp);
                    self.FillCreditRatings(resp);
                },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        }

        //Llamado API para cargar el combo de Clasificaciones Crediticias en el Agregado de Clasificaciones Crediticias por Modalidad de Ventas
        self.loadCreditRatingAvailables = function (saleModalityID) {
            if (saleModalityID !== undefined) {
                $.ajax({
                    url: "GetCreditRatingAvailables/" + saleModalityID,
                    type: "GET",
                    success: function (resp) { self.creditRatingList(resp); },
                    error: function (err) { alert("Ha ocurrido un error, por favor intente mas tarde" + err.status); }
                });
            }
            else {
                //self.saleModalityList.removeAll();
                //self.selectedProductID(undefined);
            }
        }

        //Boton AGREGAR CREDIT RATING
        self.addCreditRating = function () {
            self.showEditCreditRating(false);
            self.showAddCreditRating(true);
            self.loadCreditRatingAvailables(self.SaleModalityID());
        }

        //Boton GRABAR NUEVO
        self.saveAddCreditRating = function () {

            if (self.errors().length > 0) {
                self.errors.showAllMessages();
                return;
            }

            $.ajax({
                url: "CreateSaleModalityCreditRating",
                type: "POST",
                data: ko.toJSON(SaleModalityCreditRatingToAdd),
                contentType: "application/json",
                success: function (data) {
                    getCreditRatingBySaleModalityById(data.SaleModalityID);
                },
                error: function () {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    //resetValues();
                }
            });

            self.showEditCreditRating(false);
            self.showAddCreditRating(false);
        }

        //Boton CANCELAR ADD
        self.cancelAddCreditRating = function (creditRat) {
            self.showEditCreditRating(false);
            self.showAddCreditRating(false);
        }

        //Boton EDITAR
        self.editCreditRating = function () {
            self.toggle(self);
            self.showEditCreditRating(true);
            self.showAddCreditRating(false);
        }

        //Boton GRABAR EDICION
        self.saveEditCreditRating = function () {

            //console.log(ko.toJSON(self.CreditRatings()));

            $.ajax({
                url: "UpdateSaleModalityCreditRatingList",
                type: "POST",
                data: ko.toJSON(self.CreditRatings()),
                contentType: "application/json",
                success: function () {
                    getCreditRatingBySaleModalityById(self.SaleModalityID());
                },
                error: function () {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                }
            });

            self.toggle(self);
            self.showEditCreditRating(false);
            self.showAddCreditRating(false);
        }

        //Boton CANCELAR EDICION
        self.cancelEditCreditRating = function (creditRat) {
            self.toggle(self);
            self.showEditCreditRating(false);
            self.showAddCreditRating(false);
        }

        self.deleteCreditRating = function (creditRat) {
            //console.log("DELETE");
            //console.log(ko.toJSON(creditRat));
            //console.log(creditRat.saleModalityCreditRatingID());

            $.ajax({
                url: "api/SaleModalityCreditRatingApi/" + creditRat.saleModalityCreditRatingID(),
                type: "DELETE",
                success: function (data) {
                    getCreditRatingBySaleModalityById(creditRat.saleModalityID());
                },
                error: function (error) {
                    alert(error.status + "<--and--> " + error.statusText);
                }
            });
        }

    };


    var vm = new MSViewModel();
    vm.errors = ko.validation.group(vm);

    //https://stackoverflow.com/questions/16275700/validationoptions-not-working-with-custom-bindinghandlers
    ko.validation.makeBindingHandlerValidatable("commaDecimalFormatter");

    ko.applyBindings(vm);

    //ko.applyBindings(new MSViewModel());

})();

//http://jsfiddle.net/johnpapa/3DPvU/