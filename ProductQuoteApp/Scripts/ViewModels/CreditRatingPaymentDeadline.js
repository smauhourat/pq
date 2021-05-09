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

        self.CreditRatingID = ko.observable();
        self.CreditRatings = ko.observableArray([]);
        self.PaymentDeadlines = ko.observableArray([]);
        self.ErrorMessage = ko.observable();
        self.Description = ko.observable(); 

        self.paymentDeadlineList = ko.observableArray([]);
        self.selectedPaymentDeadlineID = ko.observable().extend({
            required: {
                params: true,
                message: "Debe seleccionar un valor de la lista."
            }
        });

        self.masterGridSelected = ko.observable(false);

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
        self.showAddPaymentDeadline = ko.observable(false);
        self.showEditPaymentDeadline = ko.observable(false);

        self.isSelected = function (data) {
            if (data.CreditRatingID === self.CreditRatingID()) {
                self.masterGridSelected(true);
                return "selected";
            }
        };

        //The SaleModality
        var CreditRating = {
            CreditRatingID: self.CreditRatingID,
            Description: self.Description
        }

        //The SaleModalityProduct
        var CreditRatingPaymentDeadline = {
            CreditRatingID: self.CreditRatingID,
            PaymentDeadlineID: self.selectedPaymentDeadlineID
        }

        loadCreditRatings();

        //Llamado API para cargar las Clasificaciones Crediticias
        function loadCreditRatings() {
            $.ajax({
                url: "GetCreditRatingsMM",
                type: "GET",
                success: function (resp) {
                    self.CreditRatings(resp);
                },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        };

        //Se muestran los PaymentDeadline por CreditRating
        self.openDetailsDialog = function (data) {
            self.CreditRatingID(data.CreditRatingID);
            self.Description(data.Description); //USed to Display SaleModality Name
            getPaymentDeadlineByCreditRating(data);
            self.showEditPaymentDeadline(false);
            self.showAddPaymentDeadline(false);
            self.template("TemplateA");
        }

        //Llamado API para cargar de los Productos con sus Costos por Modalidad de venta seleccionada.
        function getPaymentDeadlineByCreditRating(data) {
            $.ajax({
                url: "GetPaymentDeadlineByCreditRatingApi/" + data.CreditRatingID,
                type: "GET",
                success: function (resp) { self.PaymentDeadlines(resp); },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        }

        //Llamado API para cargar de los Productos con sus Costos por Modalidad de venta seleccionada (por Id).
        function getPaymentDeadlineByCreditRatingById(creditRatingID) {
            $.ajax({
                url: "GetPaymentDeadlineByCreditRatingApi/" + creditRatingID,
                type: "GET",
                success: function (resp) {
                    //alert(ko.toJSON(resp));
                    self.PaymentDeadlines(resp);
                },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        }

        //Llamado API para cargar el combo de Productos
        self.loadPaymentDeadlineAvailables = function (creditRatingID) {
            if (creditRatingID !== undefined) {
                $.ajax({
                    url: "GetPaymentDeadlineAvailables/" + creditRatingID,
                    type: "GET",
                    success: function (resp) { self.paymentDeadlineList(resp); },
                    error: function (err) { alert("Ha ocurrido un error, por favor intente mas tarde" + err.status); }
                });
            }
            else {
                //self.saleModalityList.removeAll();
                //self.selectedPaymentDeadlineID(undefined);
            }
        }

        //Boton AGREGAR PRODUCTO
        self.addPaymentDeadline = function () {
            self.showEditPaymentDeadline(false);
            self.showAddPaymentDeadline(true);
            self.loadPaymentDeadlineAvailables(self.CreditRatingID());
        }

        //Boton GRABAR EDICION
        self.saveAddPaymentDeadline = function () {
            if (self.errors().length > 0) {
                self.errors.showAllMessages();
                return;
            }
            //debug
            console.log(ko.toJSON(self.PaymentDeadlines()));

            $.ajax({
                url: "CreateCreditRatingPaymentDeadline",
                type: "POST",
                data: ko.toJSON(CreditRatingPaymentDeadline),
                contentType: "application/json",
                success: function (CreditRatingPaymentDeadline) {
                    getPaymentDeadlineByCreditRatingById(CreditRatingPaymentDeadline.CreditRatingID);
                },
                error: function () {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                }
            });

            self.showEditPaymentDeadline(false);
            self.showAddPaymentDeadline(false);
        }

        //Boton CANCELAR ADD
        self.cancelAddPaymentDeadline = function (product) {
            self.showEditPaymentDeadline(false);
            self.showAddPaymentDeadline(false);
        }

        //Boton EDITAR
        self.editPaymentDeadline = function () {
            self.toggle(self);
            self.showEditPaymentDeadline(true);
            self.showAddPaymentDeadline(false);
        }

        //Boton GRABAR NUEVO
        self.saveEditPaymentDeadline = function () {

            //debug
            console.log(ko.toJSON(self.PaymentDeadlines()));

            $.ajax({
                url: "UpdateCreditRatingPaymentDeadlineList",
                type: "POST",
                data: ko.toJSON(self.PaymentDeadlines()),
                contentType: "application/json",
                success: function () {
                    getPaymentDeadlineByCreditRatingById(self.CreditRatingID());
                },
                error: function () {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                }
            });

            self.toggle(self);
            self.showEditPaymentDeadline(false);
            self.showAddPaymentDeadline(false);
        }

        //Boton CANCELAR EDICION
        self.cancelEditPaymentDeadline = function (product) {
            self.toggle(self);
            self.showEditPaymentDeadline(false);
            self.showAddPaymentDeadline(false);
        }

        self.deletePaymentDeadline = function (paymentDeadline) {
            $.ajax({
                url: "api/CreditRatingPaymentDeadlineApi/" + paymentDeadline.CreditRatingPaymentDeadlineID,
                type: "DELETE",
                success: function () {
                    getPaymentDeadlineByCreditRatingById(paymentDeadline.CreditRatingID);
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
