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


    var GeographicAreaTransportType = function (geographicAreaTransportTypeID, geographicAreaID, geographicAreaName, transportTypeID, transportTypeDescription, freightCost) {

        var self = this;
        self.geographicAreaTransportTypeID = ko.observable(geographicAreaTransportTypeID);
        self.geographicAreaID = ko.observable(geographicAreaID);
        self.geographicAreaName = ko.observable(geographicAreaName);
        self.transportTypeID = ko.observable(transportTypeID);
        self.transportTypeDescription = ko.observable(transportTypeDescription);
        self.freightCost = ko.observable(freightCost);

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

        self.GeographicAreaID = ko.observable();
        self.GeographicAreas = ko.observableArray([]);
        self.TransportTypes = ko.observableArray([]);
        self.ErrorMessage = ko.observable();
        self.Name = ko.observable();

        self.transportTypeList = ko.observableArray([]);
        self.selectedTransportTypeID = ko.observable().extend({
            required: {
                params: true,
                message: "Debe seleccionar un valor de la lista."
            }
        });

        self.masterGridSelected = ko.observable(false);

        self.freightCostAdd = ko.observable().extend({
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

        self.FillTransportTypes = function (arg) {

            self.TransportTypes([]);
            for (var i = 0; i < arg.length; i++) {
                self.TransportTypes.push(new GeographicAreaTransportType(arg[i].GeographicAreaTransportTypeID, arg[i].GeographicAreaID, arg[i].GeographicAreaName, arg[i].TransportTypeID, arg[i].TransportTypeDescription, arg[i].FreightCost));
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // los paneles de edicion y agregado no se muestran cuando arranca
        self.showAddTransportType = ko.observable(false);
        self.showEditTransportType = ko.observable(false);

        self.isSelected = function (data) {
            if (data.GeographicAreaID === self.GeographicAreaID()) {
                self.masterGridSelected(true);
                return "selected";
            }
        };

        //The GeographicArea
        var GeographicArea = {
            GeographicAreaID: self.GeographicAreaID,
            Name: self.Name
        }

        //The GeographicAreaTransportType
        var GeographicAreaTransportTypeToAdd =  {
            GeographicAreaID: self.GeographicAreaID,
            TransportTypeID: self.selectedTransportTypeID,
            FreightCost: self.freightCostAdd
        }

        loadGeographicAreas();

        //Llamado API para cargar las Areas Geograficas
        function loadGeographicAreas() {
            $.ajax({
                url: "GetGeographicAreasMM",
                type: "GET",
                success: function (resp) {
                    self.GeographicAreas(resp);
                },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        };

        //Se muestran los Transportes con sus costos por Area Geografica seleccionada.
        self.openDetailsDialog = function (geoArea) {
            self.GeographicAreaID(geoArea.GeographicAreaID);
            self.Name(geoArea.Name); //USed to Display GeographicArea Name
            getTransportTypeByGeographicArea(geoArea);
            self.showEditTransportType(false);
            self.showAddTransportType(false);
            self.template("TemplateA");
        }

        //Llamado API para cargar de los Transportes con sus costos por Area Geografica seleccionada.
        function getTransportTypeByGeographicArea(geoArea) {
            $.ajax({
                url: "GetTransportTypeByGeographicArea/" + geoArea.GeographicAreaID,
                type: "GET",
                success: function (resp) {
                    //console.log(ko.toJSON(resp));
                    //self.TransportTypes(resp);
                    self.FillTransportTypes(resp);
                },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        }

        //Llamado API para cargar de los Transportes con sus costos por Area Geografica seleccionada (por Id).
        function getTransportTypeByGeographicAreaById(geographicAreaID) {
            $.ajax({
                url: "GetTransportTypeByGeographicArea/" + geographicAreaID,
                type: "GET",
                success: function (resp) {
                    //console.log(ko.toJSON(resp));
                    //self.TransportTypes(resp);
                    self.FillTransportTypes(resp);
                },
                error: function (err) { self.ErrorMessage("Error!!!!" + err.status); }
            });
        }

        //Llamado API para cargar el combo de Tipos Transportes en el Agregado de Transporte por Area Geografica
        self.loadTransportTypeAvailables = function (geographicAreaID) {
            if (geographicAreaID !== undefined) {
                $.ajax({
                    url: "GetTransportTypeAvailables/" + geographicAreaID,
                    type: "GET",
                    success: function (resp) { self.transportTypeList(resp); },
                    error: function (err) { alert("Ha ocurrido un error, por favor intente mas tarde" + err.status); }
                });
            }
            else {
                //self.saleModalityList.removeAll();
                //self.selectedProductID(undefined);
            }
        }

        //Boton AGREGAR TRANSPORTE
        self.addTransportType = function () {
            self.showEditTransportType(false);
            self.showAddTransportType(true);
            self.loadTransportTypeAvailables(self.GeographicAreaID());
        }

        //Boton GRABAR NUEVO
        self.saveAddTransportType = function () {
            if (self.errors().length > 0) {
                self.errors.showAllMessages();
                return;
            }
            //debug
            //console.log("SAVE ADD");
            //console.log(ko.toJSON(GeographicAreaTransportTypeToAdd));

            $.ajax({
                url: "CreateGeographicAreaTransportType",
                type: "POST",
                data: ko.toJSON(GeographicAreaTransportTypeToAdd),
                contentType: "application/json",
                success: function (data) {
                    getTransportTypeByGeographicAreaById(data.GeographicAreaID);
                },
                error: function () {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    resetValues();
                }
            });

            self.showEditTransportType(false);
            self.showAddTransportType(false);
        }

        //Boton CANCELAR ADD
        self.cancelAddTransportType = function (transTpe) {
            self.showEditTransportType(false);
            self.showAddTransportType(false);
        }

        //Boton EDITAR
        self.editTransportType = function () {
            self.toggle(self);
            self.showEditTransportType(true);
            self.showAddTransportType(false);
        }

        //Boton GRABAR EDICION
        self.saveEditTransportType = function () {
            //debug
            //console.log("SAVE EDIT");
            //console.log(ko.toJSON(self.TransportTypes()));

            $.ajax({
                url: "UpdateGeographicAreaTransportTypeList",
                type: "POST",
                data: ko.toJSON(self.TransportTypes()),
                contentType: "application/json",
                success: function () {
                    getTransportTypeByGeographicAreaById(self.GeographicAreaID());
                },
                error: function () {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                }
            });

            self.toggle(self);
            self.showEditTransportType(false);
            self.showAddTransportType(false);
        }

        //Boton CANCELAR EDICION
        self.cancelEditTransportType = function (transTpe) {
            self.toggle(self);
            self.showEditTransportType(false);
            self.showAddTransportType(false);
        }

        //falta probar
        self.deleteTransportType = function (transTpe) {
            
            $.ajax({
                url: "api/GeographicAreaTransportTypeApi/" + transTpe.geographicAreaTransportTypeID(),
                type: "DELETE",
                success: function (data) {
                    getTransportTypeByGeographicAreaById(transTpe.geographicAreaID());
                },
                error: function (error) {
                    alert(error.status + "<--and--> " + error.statusText);
                }
            });
        }

    };
    var vm = new MSViewModel();
    vm.errors = ko.validation.group(vm);

    ko.validation.makeBindingHandlerValidatable("commaDecimalFormatter");

    ko.applyBindings(vm);

    //ko.applyBindings(new MSViewModel());

})();

///http://jsbin.com/ipijet/5/edit?html,js,output

///Deberia hacer otro ViewModel para cada elemnto de la grilla editable de abajo