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

    Number.isInteger = Number.isInteger || function (value) {
        return typeof value === 'number' &&
          isFinite(value) &&
          Math.floor(value) === value;
    };

    var validateMinimumQuantityDelivery = function (val) {
        if ((val == 0) || (!Number.isInteger(val)))
            return false;
        else
            return true;
    };

    var validateIntegerPositive = function (val) {
        val = (val + "").replace(",", ".")
        val = parseFloat(val);
        if ((val == 0) || (!Number.isInteger(val)))
            return false;
        else
            return true;
    };

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

    var validateFloatPositiveNoZero = function (val) {
        val = (val + "").replace(",", ".").replace(".", "")
        val = parseFloat(val);
        if ((!Number.isInteger(val)) || val < 0)
            return false;
        if ((val == 0) || (val < 0))
            return false;
        else
            return true;
    };

    var validateCustomerCompanyName = function (val) {
        if (val == undefined)
            return false;
        else
            return true;
    };

    var validateValidityOfPrice = function (val) {
        if ((val !== undefined) && (val < new Date()))
            return false;
        else
            return true;
    };

    var ProductQuoteExpressViewModel = function () {

        var self = this;

        self.template = ko.observable("templateA");

        self.customerCompanyNameValidateTrue = ko.observable(false);

        self.canCalculateProductQuote = ko.observable(true);
        self.canCreateProductQuote = ko.observable(false);
        self.canShowMaximumMonthsStock = ko.observable(true);
        self.canShowMaximumMonthsStock2 = ko.observable(true);
        self.canShowCosteo = ko.observable(false);
        self.showCosteo = ko.observable(false);


        self.currentUserIsSellerUser = ko.observable($('#myHDCurrentUserIsSellerUser').val());
        self.currentUserEditGlobalVariables = ko.observable($('#myHDCurrentUserEditGlobalVariables').val());
        self.currentUserEditMarginOrPrice = ko.observable($('#myHDCurrentUserEditMarginOrPrice').val());
        self.currentUserSeeCosting = ko.observable($('#myHDCurrentUserSeeCosting').val());
        
        self.customerID = ko.observable();
        self.currentCustomerID = ko.observable($('#myHDCurrentCustomerID').val());
        self.userId = ko.observable($('#myHDUserId').val());
        self.customerCreditRatingID = ko.observable();
        self.customerIsSpot = ko.observable();
        self.customerContactName = ko.observable();
        self.customerCompanyName = ko.observable().extend({
            required: {
                params: true,
                message: "Debe ingresar el 'Nombre de la Empresa'",
                onlyIf: function () { return self.customerCompanyNameValidateTrue(); }
            },
            validation: {
                validator: validateCustomerCompanyName,
                message: "Debe ingresar el 'Nombre de la Empresa'",
                onlyIf: function () { return self.customerCompanyNameValidateTrue(); }
            }
        });
        self.customerCompanyName.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });

        self.customerDelayAverageDays = ko.observable();
        self.costoFleteInput = ko.observable();
        self.precioInput = ko.observable();
        self.margenInput = ko.observable();
        self.margenUSDInput = ko.observable();
        self.sellerCommissionInput = ko.observable();
        var currentD = new Date();
        currentD.setDate(currentD.getDate() + 1);
        //var currentDate = (new Date()).toISOString().split('T')[0];
        var currentDate = currentD.toISOString().split('T')[0];

        self.productValidityOfPriceInput = ko.observable(currentDate).extend({
            validation: {
                validator: validateValidityOfPrice,
                message: 'La Fecha de validez debe ser superior al dia corriente.'
            }
        });

        if (self.currentUserIsSellerUser() == "value")
            self.isSellerUser = ko.observable(true);
        else
            self.isSellerUser = ko.observable(false);

        self.factorRofexInput = ko.observable();
        self.deliveryAddress = ko.observable();
        self.availabilityDays = ko.observable();
        self.datesDeliveryInput = ko.observable();
        self.userObservations = ko.observable();

        self.currentUserFullName = ko.observable($('#myHDCurrentUserFullName').val());
        self.currentUserEmail = ko.observable($('#myHDCurrentUserEmail').val());
        self.currenCustomerName = ko.observable($('#myHDCurrenCustomerName').val());
        self.currentCustomerCompany = ko.observable($('#myHDCurrentCustomerCompany').val());

        //no se esta usando
        ko.bindingHandlers.numericText = {
            update: function(element, valueAccessor, allBindingsAccessor) {
               var value = ko.utils.unwrapObservable(valueAccessor()),
                   precision = ko.utils.unwrapObservable(allBindingsAccessor().precision) || ko.bindingHandlers.numericText.defaultPrecision,
                   formattedValue = value.toFixed(precision);
        
                ko.bindingHandlers.text.update(element, function() { return formattedValue; });
            },
            defaultPrecision: 1  
        };


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

        //ko.bindingHandlers.Date = {
        //    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        //        var value = ko.utils.unwrapObservable(valueAccessor()),
        //            allBindings = allBindingsAccessor();

        //        var format = allBindings.format || 'MM/DD/YYYY';

        //        if (value !== null && value !== '') {
        //            $(element).html(moment(value).format(format));
        //        }
        //    },

        //    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        //        var val = ko.utils.unwrapObservable(valueAccessor());
        //        $(element).val(val);
        //    }
        //};

        self.customersList = ko.observableArray([]);
        self.selectedCustomer = ko.observable().extend({
            required: {
                params: true,
                message: "Debe seleccionar un valor de la lista."
            }
        });

        self.salesChannelsUserList = ko.observableArray([]);
        self.selectedSalesChannelID = ko.observable().extend({
            required: {
                params: true,
                message: "Debe seleccionar un valor de la lista."
            }
        });

        self.productsList = ko.observableArray([]);
        self.selectedProductID = ko.observable().extend({
            required: {
                params: true,
                message: "Debe seleccionar un valor de la lista."
            }
        });

        //self.saleModalityList = ko.observableArray([]);
        //self.selectedSaleModalityID = ko.observable().extend({
        //    required: {
        //        params: true,
        //        message: "Debe seleccionar un valor de la lista."
        //    }
        //});

        //self.geographicAreaList = ko.observableArray([]);
        //self.selectedGeographicArea = ko.observable().extend({
        //    required: {
        //        params: true,
        //        message: "Debe seleccionar un valor de la lista."
        //    }
        //});

        self.paymentDeadlineList = ko.observableArray([]);
        self.selectedPaymentDeadline = ko.observable().extend({
            required: {
                params: true,
                message: "Debe seleccionar un valor de la lista."
            }
        });

        //ojoo
        self.maximumMonthsStock = ko.observable().extend({
            numeric: 0
        });

        self.quantityOpenPurchaseOrder = ko.observable().extend({
            numeric: 0
        });

        self.minimumQuantityDelivery = ko.observable().extend({
            validation: {
                validator: validateMinimumQuantityDelivery,
                message: 'Debe ser un número entero mayor a cero.'
            }
        });

        //self.exchangeTypeList = ko.observableArray([]);
        //self.selectedExchangeType = ko.observable().extend({
        //    required: {
        //        params: true,
        //        message: "Debe seleccionar un valor de la lista."
        //    }
        //});

        //self.maximumMonthsStockList = ko.observableArray([]);
        //self.selectedMaximumMonthsStock = ko.observable().extend({
        //    required: {
        //        params: true,
        //        message: "Debe seleccionar un valor de la lista."
        //    }
        //});

        self.deliveryAmount = ko.observable();
        //self.deliveryAmountList = ko.observableArray([]);
        //self.selectedDeliveryAmount = ko.observable().extend({
        //    required: {
        //        params: true,
        //        message: "Debe seleccionar un valor de la lista."
        //    }
        //});

        self.price = ko.observable();
        self.priceOriginal = ko.observable();
        
        self.customerTotalCost = ko.observable();

        self.costoProducto_PorTON = ko.observable();
        self.typeChange = ko.observable();
        self.flete_PorITEM = ko.observable();
        self.flete_PorTON = ko.observable();
        self.almacenamientoCosto_PorITEM = ko.observable();
        self.almacenamientoCosto_PorTON = ko.observable();
        self.inout_PorITEM = ko.observable();
        self.inout_PorTON = ko.observable();
        self.merma_PorITEM = ko.observable();
        self.merma_PorTON = ko.observable();
        self.costoFinancieroMensual_PorITEM = ko.observable();
        self.costoFinancieroMensual_PorTON = ko.observable();

        self.exchangeInsurance = ko.observable();
        self.exchangeInsurance_PorTON = ko.observable();

        self.costoFin_PorTON = ko.observable();
        self.gastosFijos_PorITEM = ko.observable();
        self.gastosFijos_PorTON = ko.observable();
        self.impuestoDC_PorITEM = ko.observable();
        self.impuestoDC_PorTON = ko.observable();
        self.iibbAlicuota_PorITEM = ko.observable();
        self.iibbAlicuota_PorTON = ko.observable();
        self.comisiones_PorITEM = ko.observable();
        self.comisiones_PorTON = ko.observable();
        self.margenNetoUSD_PorTON = ko.observable();
        self.margenNetoOriginalUSD_PorTON = ko.observable();
        self.margenNetoPorc_PorTON = ko.observable();
        self.margenNetoOriginalPorc_PorTON = ko.observable();
        self.margenNetoEntidadOrigen = ko.observable();
        self.factorRofex = ko.observable();
        self.valorRofex = ko.observable();
        self.tiempoMedioStockMeses = ko.observable();
        self.leyendaCalculoCostoTransporte = ko.observable();
        self.message = ko.observable();
        self.observations = ko.observable();
        
        self.gv_CostoFinancieroMensual = ko.observable();
        self.gv_CostoAlmacenamientoMensual = ko.observable();
        self.gv_FactorCostoAlmacenamientoMensual = ko.observable();
        self.gv_DiasStockPromedioDistLocal = ko.observable();
        self.freightType = ko.observable();
        self.precioVentaRofex = ko.observable();
        self.margenNetoUSDRofex = ko.observable();
        self.margenNetoPORCRofex = ko.observable();
        self.workingCapital = ko.observable();
        self.costoFinancieroMensual_PorITEM_Formula = ko.observable();
        self.workingCapital_Formula = ko.observable();
        self.tiempoMedioStockDias_Formula = ko.observable();
        self.hasWayOfException = ko.observable();
        self.exceptionApplyType = ko.observable();
        self.wayOfExceptionValue = ko.observable();

        self.isSaleModalityFindParam = ko.observable();
        self.isGeographicAreaFindParam = ko.observable();
        self.isPaymentDeadlineFindParam = ko.observable();
        self.isQuantityOpenPurchaseOrderFindParam = ko.observable();
        self.isDeliveryAmountFindParam = ko.observable();
        self.isMaximumMonthsStockFindParam = ko.observable();
        self.isExchangeTypeFindParam = ko.observable();
        self.isMinimumQuantityDeliveryFindParam = ko.observable();

        ////////////////////////////////////////////////////////////////
        // DETALLE POR ENTREGA
        ////////////////////////////////////////////////////////////////

        self.hasWayOfException_View = ko.computed(function () {
            if (self.hasWayOfException())
                return 'SI';
            else
                return 'NO';
        });

        self.exceptionApplyType_View = ko.computed(function () {
            if (self.exceptionApplyType() == 1)
                return 'Precio Proporcional';
            if (self.exceptionApplyType() == 2)
                return 'Margen Equivalente';
            if (self.exceptionApplyType() == 3)
                return 'Precio Fijo';
        });

        self.wayOfExceptionValue_View = ko.computed(function () {
            var nu = parseFloat(self.wayOfExceptionValue()).toFixed(4);
            return nu.toString().replace('.', ',');
        });

        self.captionWayOfException = ko.computed(function () {
            if (self.hasWayOfException())
                return 'SI  - ' + self.exceptionApplyType_View() + ' - ' + self.wayOfExceptionValue_View();
            else
                return 'NO';
        });

        self.costoProducto_PorEntrega = ko.computed(function () {
            return self.costoProducto_PorTON() * self.minimumQuantityDelivery();
        });
        self.flete_PorEntrega = ko.computed(function () {
            return (self.flete_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.almacenamientoCosto_PorEntrega = ko.computed(function () {
            return parseFloat(self.almacenamientoCosto_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.inout_PorEntrega = ko.computed(function () {
            return parseFloat(self.inout_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.merma_PorEntrega = ko.computed(function () {
            return parseFloat(self.merma_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.costoFinancieroMensual_PorEntrega = ko.computed(function () {
            return parseFloat(self.costoFinancieroMensual_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.exchangeInsurance_PorEntrega = ko.computed(function () {
            return parseFloat(self.exchangeInsurance_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.gastosFijos_PorEntrega = ko.computed(function () {
            return parseFloat(self.gastosFijos_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.impuestoDC_PorEntrega = ko.computed(function () {
            return parseFloat(self.impuestoDC_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.iibbAlicuota_PorEntrega = ko.computed(function () {
            return parseFloat(self.iibbAlicuota_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.comisiones_PorEntrega = ko.computed(function () {
            return parseFloat(self.comisiones_PorTON() * self.minimumQuantityDelivery()).toFixed(2);
        });
        //ojo, se quitaron decimales 10/09/2020
        self.customerTotalCost_PorEntrega = ko.computed(function () {
            var ret = parseFloat(self.costoProducto_PorEntrega()) + parseFloat(self.flete_PorEntrega()) + parseFloat(self.almacenamientoCosto_PorEntrega()) + parseFloat(self.inout_PorEntrega()) + parseFloat(self.merma_PorEntrega()) + parseFloat(self.costoFinancieroMensual_PorEntrega()) + parseFloat(self.exchangeInsurance_PorEntrega()) + parseFloat(self.gastosFijos_PorEntrega()) + parseFloat(self.impuestoDC_PorEntrega()) + parseFloat(self.iibbAlicuota_PorEntrega()) + parseFloat(self.comisiones_PorEntrega());
            return parseFloat(ret).toFixed(0);
        });
        self.price_PorEntrega = ko.computed(function () {
            return self.price() * self.minimumQuantityDelivery();
        });
        self.priceOriginal_PorEntrega = ko.computed(function () {
            return parseFloat((self.priceOriginal() * self.minimumQuantityDelivery()).toFixed(2));
        });
        self.margenNetoUSD_PorEntrega = ko.computed(function () {
            return self.price_PorEntrega() - self.customerTotalCost_PorEntrega();
        });
        self.margenNetoOriginalUSD_PorEntrega = ko.computed(function () {
            return self.priceOriginal_PorEntrega() - self.customerTotalCost_PorEntrega();
        });
        self.margenNetoPorc_PorEntrega = ko.computed(function () {
            return ((self.margenNetoUSD_PorEntrega() / self.price_PorEntrega()) * 100).toFixed(2);
        });
        self.margenNetoOriginalPorc_PorEntrega = ko.computed(function () {
            return ((self.margenNetoOriginalUSD_PorEntrega() / self.priceOriginal_PorEntrega()) * 100).toFixed(2);
        });
        self.precioVentaRofex_PorEntrega = ko.computed(function () {
            return parseFloat(self.precioVentaRofex() * self.minimumQuantityDelivery()).toFixed(2);
        });
        self.margenNetoUSDRofex_PorEntrega = ko.computed(function () {
            return self.precioVentaRofex_PorEntrega() - self.customerTotalCost_PorEntrega();
        });
        self.margenNetoPORCRofex_PorEntrega = ko.computed(function () {
            return ((self.margenNetoUSDRofex_PorEntrega() / self.precioVentaRofex_PorEntrega()) * 100).toFixed(2);
        });
        
        
        ////////////////////////////////////////////////////////////////
        // DETALLE POR ORDEN COMPRA
        ////////////////////////////////////////////////////////////////

        self.costoProducto_PorOC = ko.computed(function () {
            return self.costoProducto_PorTON() * self.quantityOpenPurchaseOrder();
        });
        self.flete_PorOC = ko.computed(function () {
            return parseFloat(self.flete_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.almacenamientoCosto_PorOC = ko.computed(function () {
            return parseFloat(self.almacenamientoCosto_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.inout_PorOC = ko.computed(function () {
            return parseFloat(self.inout_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.merma_PorOC = ko.computed(function () {
            return parseFloat(self.merma_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.costoFinancieroMensual_PorOC = ko.computed(function () {
            return parseFloat(self.costoFinancieroMensual_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.exchangeInsurance_PorOC = ko.computed(function () {
            return parseFloat(self.exchangeInsurance_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.gastosFijos_PorOC = ko.computed(function () {
            return parseFloat(self.gastosFijos_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.impuestoDC_PorOC = ko.computed(function () {
            return parseFloat(self.impuestoDC_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.iibbAlicuota_PorOC = ko.computed(function () {
            return parseFloat(self.iibbAlicuota_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.comisiones_PorOC = ko.computed(function () {
            return parseFloat(self.comisiones_PorTON() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        //ojo, se quitaron decimales (10/09/2020)
        self.customerTotalCost_PorOC = ko.pureComputed(function () {
            var ret = parseFloat(self.costoProducto_PorOC()) + parseFloat(self.flete_PorOC()) + parseFloat(self.almacenamientoCosto_PorOC()) + parseFloat(self.inout_PorOC()) + parseFloat(self.merma_PorOC()) + parseFloat(self.costoFinancieroMensual_PorOC()) + parseFloat(self.exchangeInsurance_PorOC()) + parseFloat(self.gastosFijos_PorOC()) + parseFloat(self.impuestoDC_PorOC()) + parseFloat(self.iibbAlicuota_PorOC()) + parseFloat(self.comisiones_PorOC())
            return parseFloat(ret).toFixed(0);
        });
        self.price_PorOC = ko.computed(function () {
            return self.price() * self.quantityOpenPurchaseOrder();
        });
        self.priceOriginal_PorOC = ko.computed(function () {
            return parseFloat((self.priceOriginal() * self.quantityOpenPurchaseOrder()).toFixed(2));
        });
        self.margenNetoUSD_PorOC = ko.computed(function () {
            return self.price_PorOC() - self.customerTotalCost_PorOC();
        });
        self.margenNetoOriginalUSD_PorOC = ko.computed(function () {
            return self.priceOriginal_PorOC() - self.customerTotalCost_PorOC();
        });
        self.margenNetoPorc_PorOC = ko.computed(function () {
            return ((self.margenNetoUSD_PorOC() / self.price_PorOC()) * 100).toFixed(2);
        });
        self.margenNetoOriginalPorc_PorOC = ko.computed(function () {
            return ((self.margenNetoOriginalUSD_PorOC() / self.priceOriginal_PorOC()) * 100).toFixed(2);
        });

        self.precioVentaRofex_PorOC = ko.computed(function () {
            return parseFloat(self.precioVentaRofex() * self.quantityOpenPurchaseOrder()).toFixed(2);
        });
        self.margenNetoUSDRofex_PorOC = ko.computed(function () {
            return parseFloat(self.precioVentaRofex_PorOC() - self.customerTotalCost_PorOC()).toFixed(2);
        });
        self.margenNetoPORCRofex_PorOC = ko.computed(function () {
            return ((self.margenNetoUSDRofex_PorOC() / self.precioVentaRofex_PorOC()) * 100).toFixed(2);
        });
        self.margenNetoUSDRofex_View = ko.computed(function () {
            return parseFloat(self.margenNetoUSDRofex()).toFixed(5);
        });
        self.margenNetoOriginalUSD_PorEntrega_View = ko.computed(function () {
            return parseFloat(self.margenNetoOriginalUSD_PorEntrega()).toFixed(2);
        });
        self.margenNetoOriginalUSD_PorOC_View = ko.computed(function () {
            return parseFloat(self.margenNetoOriginalUSD_PorOC()).toFixed(2);
        });
        self.margenNetoUSDRofex_PorEntrega_View = ko.computed(function () {
            return parseFloat(self.margenNetoUSDRofex_PorEntrega()).toFixed(2);
        });
        self.margenNetoUSDRofex_PorOC_View = ko.computed(function () {
            return parseFloat(self.margenNetoUSDRofex_PorOC()).toFixed(2);
        });
        self.costoFinancieroMensual_PorITEM_View = ko.computed(function () {
            return parseFloat(self.costoFinancieroMensual_PorITEM()).toFixed(2);
        });

        self.exchangeInsurance_View = ko.computed(function () {
            return parseFloat(self.exchangeInsurance()).toFixed(2);
        });

        self.exchangeInsurance_PorTON_View = ko.computed(function () {
            return parseFloat(self.exchangeInsurance_PorTON()).toFixed(5);
        });

        ////////////////////////////////////////////////////////////////
        // PRODUCT PROPERTIES
        ////////////////////////////////////////////////////////////////
        self.productName = ko.observable();
        self.productSingleName = ko.observable();
        self.productProviderName = ko.observable();
        self.productBrandName = ko.observable();
        self.productFCLKilogram = ko.observable();
        self.productPackagingName = ko.observable();
        self.productPackagingKilogram = ko.observable();
        self.productPositionKilogram = ko.observable();
        self.productClientObservations = ko.observable();
        self.productValidityOfPrice = ko.observable();
        self.productValidityOfPrice_view = ko.observable();
        self.productWaste = ko.observable();
        self.productProviderPaymentDeadline = ko.observable();
        self.productLeadTime = ko.observable();
        self.productBuyAndSellDirect = ko.observable();
        self.productInOutStorage = ko.observable();
        self.productSingleName = ko.observable();
        self.productDocuments = ko.observableArray([]);
        self.productPackagingID = ko.observable();
        self.productSellerCompanyName = ko.observable();

        self.product = function (data) {
            self.productName(data.FullName);
            self.productSingleName(data.Name);
            self.productProviderName(data.ProviderName);
            self.productBrandName(data.Brand);
            self.productFCLKilogram(data.FCLKilogram);

            //Valor por defecto de la Cantidad Total Orden de Compra
            self.quantityOpenPurchaseOrder(data.FCLKilogram);

            self.productPositionKilogram(data.PositionKilogram);
            self.productClientObservations(data.ClientObservations);
            self.productPackagingKilogram(data.PackagingKilogram);
            self.productValidityOfPrice(data.ValidityOfPrice != undefined ? moment(data.ValidityOfPrice).format('YYYY-MM-DD') : "");
            self.productValidityOfPrice_view(data.ValidityOfPrice != undefined ? moment(data.ValidityOfPrice).format('DD-MM-YYYY').toString().replace("-", "/").replace("-", "/") : "");
            self.productWaste(data.Waste);
            self.productProviderPaymentDeadline(data.ProviderPaymentDeadline);
            self.productLeadTime(data.LeadTime);
            self.productBuyAndSellDirect(data.BuyAndSellDirect);
            self.productInOutStorage(data.InOutStorage);
            self.productSingleName(data.Name);
            self.productPackagingID(data.PackagingID);
            self.productPackagingName(data.PackagingDescription);

            self.productDocuments(data.ProductDocuments != undefined ? data.ProductDocuments : "");

            self.productSellerCompanyName(data.SellerCompanyName);
        };
        ////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////
        // GLOBAL VARIABLES PROPERTIES
        ////////////////////////////////////////////////////////////////
        self.gvd_costoAlmacenamientoMensual = ko.observable().extend({
            validation: {
                validator: validateFloatPositive,
                message: 'Debe ser un número entero mayor o igual a cero.'
            }
        });
        self.gvd_costoInOut = ko.observable().extend({
            validation: {
                validator: validateFloatPositive,
                message: 'Debe ser un número entero mayor o igual a cero.'
            }
        });
        self.gvd_costoFinancieroMensual = ko.observable().extend({
            validation: {
                validator: validateFloatPositive,
                message: 'Debe ser un número entero mayor o igual a cero.'
            }
        });
        self.gvd_impuestoDebitoCredito = ko.observable().extend({
            validation: {
                validator: validateFloatPositive,
                message: 'Debe ser un número entero mayor o igual a cero.'
            }
        });
        self.gvd_gastosFijos = ko.observable().extend({
            validation: {
                validator: validateFloatPositive,
                message: 'Debe ser un número entero mayor o igual a cero.'
            }
        });
        self.gvd_iIBBAlicuota = ko.observable().extend({
            validation: {
                validator: validateFloatPositive,
                message: 'Debe ser un número entero mayor o igual a cero.'
            }
        });
        self.gvd_tipoCambio = ko.observable().extend({
            numeric: 3,
            validation: {
                validator: validateFloatPositiveNoZero,
                message: "Debe ser un número mayor a cero."
            }
        });
        //validateIntegerPositive,
        self.gvd_factorCostoAlmacenamientoMensual = ko.observable().extend({
            validation: {
                validator: validateFloatPositive,
                message: 'Debe ser un número mayor a cero.'
            }
        });
        self.gvd_diasStockPromedioDistLocal = ko.observable();

        self.globalvariables = function (data) {
            self.gvd_costoAlmacenamientoMensual(data.CostoAlmacenamientoMensual.toFixed(0));
            self.gvd_costoInOut(data.CostoInOut.toFixed(0));
            self.gvd_costoFinancieroMensual(data.CostoFinancieroMensual.toFixed(2));
            self.gvd_impuestoDebitoCredito(data.ImpuestoDebitoCredito.toFixed(2));
            self.gvd_gastosFijos(data.GastosFijos.toFixed(2));
            self.gvd_iIBBAlicuota(data.IIBBAlicuota.toFixed(2));
            self.gvd_tipoCambio(data.TipoCambio.toFixed(3));
            self.gvd_factorCostoAlmacenamientoMensual(data.FactorCostoAlmacenamientoMensual);
            self.gvd_diasStockPromedioDistLocal(data.DiasStockPromedioDistLocal);
        }

        self.gvd_costoAlmacenamientoMensual.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.gvd_costoInOut.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.gvd_costoFinancieroMensual.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.gvd_impuestoDebitoCredito.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.gvd_gastosFijos.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.gvd_iIBBAlicuota.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.gvd_tipoCambio.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.gvd_factorCostoAlmacenamientoMensual.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        }); 
        self.gvd_diasStockPromedioDistLocal.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.costoFleteInput.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });

        //////////////////////////////////////////////////////////////////
        self.precioInput.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);

            if ((value !== undefined) && (value !== "") && (value !== null))
            {
                self.margenInput(null);
                self.margenUSDInput(null);
            }
        });

        self.margenInput.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);

            if ((value !== undefined) && (value !== "") && (value !== null))
            {
                self.precioInput(null);
                self.margenUSDInput(null);
            }
        });

        self.margenUSDInput.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);

            if ((value !== undefined) && (value !== "") && (value !== null)) {
                self.precioInput(null);
                self.margenInput(null);
            }
        });
        //////////////////////////////////////////////////////////////////

        self.sellerCommissionInput.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.productValidityOfPriceInput.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });
        self.factorRofexInput.subscribe(function (value) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });

        ////////////////////////////////////////////////////////////////
        // SALE MODALITY PROPERTIES
        ////////////////////////////////////////////////////////////////
        //self.saleModalityDescription = ko.observable();
        //self.saleModalityResume = ko.observable();

        //self.saleModality = function (data) {
        //    self.saleModalityDescription(data.Description);
        //    self.saleModalityResume(data.Resume);
        //};
        ////////////////////////////////////////////////////////////////

        self.geographicAreaID = ko.observable();
        self.geographicAreaName = ko.observable();
        self.paymentDeadlineID = ko.observable();
        self.paymentDeadlineName = ko.observable();
        self.paymentDeadlineMonths = ko.observable();
        self.paymentDeadlineDays = ko.observable();

        self.exchangeTypeID = ko.observable();
        self.exchangeTypeName = ko.observable();
        self.exchangeTypeLargeDescription = ko.observable();

        ////////////////////////////////////////////////////////////////
        //The Object which stored data entered in the observables
        ////////////////////////////////////////////////////////////////

        var productQuote = {
            SalesChannelID: self.selectedSalesChannelID,
            CustomerID: self.customerID,
            UserId: self.userId,
            CustomerName: self.currenCustomerName,
            CustomerContactName: self.customerContactName,
            CustomerContactMail: self.currentUserEmail,
            CustomerCompany: self.customerCompanyName,
            CustomerDelayAverageDays: self.customerDelayAverageDays,
            ProductID: self.selectedProductID,
            ProductName: self.productName,
            ProductProviderName: self.productProviderName,
            ProductBrandName: self.productBrandName,
            ProductFCLKilogram: self.productFCLKilogram,
            ProductPackagingID: self.productPackagingID,
            ProductPackagingName: self.productPackagingName,
            ProductValidityOfPrice: self.productValidityOfPrice,
            ProductWaste: self.productWaste,
            ProductProviderPaymentDeadline: self.productProviderPaymentDeadline,
            ProductLeadTime: self.productLeadTime,
            ProductBuyAndSellDirect: self.productBuyAndSellDirect,
            ProductInOutStorage: self.productInOutStorage,
            ProductSingleName: self.productSingleName,
            ProductSellerCompanyName: self.productSellerCompanyName,
            //SaleModalityID: self.selectedSaleModalityID,
            //SaleModalityName: self.saleModalityDescription,
            SaleModalityID: 1,
            SaleModalityName: 'Distribución Local',
            MaximumMonthsStock: self.maximumMonthsStock,
            //DeliveryAmount: self.deliveryAmount,
            DeliveryAmount: 1,
            MinimumQuantityDelivery: self.minimumQuantityDelivery,
            QuantityOpenPurchaseOrder: self.quantityOpenPurchaseOrder,
            //GeographicAreaID: self.geographicAreaID,
            //GeographicAreaName: self.geographicAreaName,
            GeographicAreaID: 7,
            GeographicAreaName: 'A Retirar',
            PaymentDeadlineID: self.paymentDeadlineID,
            PaymentDeadlineName: self.paymentDeadlineName,
            PaymentDeadlineMonths: self.paymentDeadlineMonths,
            //ExchangeTypeID: self.exchangeTypeID,
            //ExchangeTypeName: self.exchangeTypeName,
            //ExchangeTypeLargeDescription: self.exchangeTypeLargeDescription,
            ExchangeTypeID: 1,
            ExchangeTypeName: 'Dólares',
            ExchangeTypeLargeDescription: 'Con variación Tipo Cambio',

            Price: self.price,
            PriceOriginal: self.priceOriginal,
            CustomerTotalCost: self.customerTotalCost,
            CostoProducto_PorTON: self.costoProducto_PorTON,
            TypeChange: self.typeChange,
            Flete_PorITEM: self.flete_PorITEM,
            Flete_PorTON: self.flete_PorTON,
            AlmacenamientoCosto_PorITEM: self.almacenamientoCosto_PorITEM,
            AlmacenamientoCosto_PorTON: self.almacenamientoCosto_PorTON,
            Inout_PorITEM: self.inout_PorITEM,
            Inout_PorTON: self.inout_PorTON,
            Merma_PorITEM: self.merma_PorITEM,
            Merma_PorTON: self.merma_PorTON,
            CostoFinancieroMensual_PorITEM: self.costoFinancieroMensual_PorITEM,
            CostoFinancieroMensual_PorTON: self.costoFinancieroMensual_PorTON,

            ExchangeInsurance: self.exchangeInsurance,
            ExchangeInsurance_PorTON: self.exchangeInsurance_PorTON,

            CostoFin_PorTON: self.costoFin_PorTON,
            GastosFijos_PorITEM: self.gastosFijos_PorITEM,
            GastosFijos_PorTON: self.gastosFijos_PorTON,
            ImpuestoDC_PorITEM: self.impuestoDC_PorITEM,
            ImpuestoDC_PorTON: self.impuestoDC_PorTON,
            IIBBAlicuota_PorITEM: self.iibbAlicuota_PorITEM,
            IIBBAlicuota_PorTON: self.iibbAlicuota_PorTON,
            Comisiones_PorITEM: self.comisiones_PorITEM,
            Comisiones_PorTON: self.comisiones_PorTON,
            MargenNetoUSD_PorTON: self.margenNetoUSD_PorTON,
            MargenNetoOriginalUSD_PorTON: self.margenNetoOriginalUSD_PorTON,
            MargenNetoPorc_PorTON: self.margenNetoPorc_PorTON,
            MargenNetoOriginalPorc_PorTON: self.margenNetoOriginalPorc_PorTON,
            MargenNetoEntidadOrigen: self.margenNetoEntidadOrigen,
            FactorRofex: self.factorRofex,
            ValorRofex: self.valorRofex,
            TiempoMedioStockMeses: self.tiempoMedioStockMeses,
            LeyendaCalculoCostoTransporte: self.leyendaCalculoCostoTransporte,
            Message: self.message,
            Observations: self.observations,
            

            GV_CostoFinancieroMensual: self.gv_CostoFinancieroMensual,
            GV_CostoAlmacenamientoMensual: self.gv_CostoAlmacenamientoMensual,
            GV_FactorCostoAlmacenamientoMensual: self.gv_FactorCostoAlmacenamientoMensual,
            GV_DiasStockPromedioDistLocal: self.gv_DiasStockPromedioDistLocal,
            FreightType: self.freightType,
            PrecioVentaRofex: self.precioVentaRofex,
            MargenNetoUSDRofex: self.margenNetoUSDRofex,
            MargenNetoPORCRofex: self.margenNetoPORCRofex,
            CostoFinancieroMensual_PorITEM_Formula: self.costoFinancieroMensual_PorITEM_Formula,
            WorkingCapital_Formula: self.workingCapital_Formula,
            TiempoMedioStockDias_Formula: self.tiempoMedioStockDias_Formula,

            GVD_CostoAlmacenamientoMensual: self.gvd_costoAlmacenamientoMensual,
            GVD_CostoInOut: self.gvd_costoInOut,
            GVD_CostoFinancieroMensual: self.gvd_costoFinancieroMensual,
            GVD_ImpuestoDebitoCredito: self.gvd_impuestoDebitoCredito,
            GVD_GastosFijos: self.gvd_gastosFijos,
            GVD_IIBBAlicuota: self.gvd_iIBBAlicuota,
            GVD_TipoCambio: self.gvd_tipoCambio,
            GVD_FactorCostoAlmacenamientoMensual: self.gvd_factorCostoAlmacenamientoMensual,
            GVD_DiasStockPromedioDistLocal: self.gvd_diasStockPromedioDistLocal,

            CostoFleteInput: self.costoFleteInput,
            PrecioInput: self.precioInput,
            MargenInput: self.margenInput,
            MargenUSDInput: self.margenUSDInput,
            SellerCommissionInput: self.sellerCommissionInput,
            ProductValidityOfPriceInput: self.productValidityOfPriceInput,
            IsSellerUser: self.isSellerUser,
            FactorRofexInput: self.factorRofexInput,
            DeliveryAddress: self.deliveryAddress,
            AvailabilityDays: self.availabilityDays,
            DatesDeliveryInput: self.datesDeliveryInput,
            UserObservations: self.userObservations,

            HasWayOfException: self.hasWayOfException,
            ExceptionApplyType: self.exceptionApplyType,
            WayOfExceptionValue: self.wayOfExceptionValue,

            IsSaleModalityFindParam: self.isSaleModalityFindParam,
            IsGeographicAreaFindParam: self.isGeographicAreaFindParam,
            IsPaymentDeadlineFindParam: self.isPaymentDeadlineFindParam,
            IsQuantityOpenPurchaseOrderFindParam: self.isQuantityOpenPurchaseOrderFindParam,
            IsDeliveryAmountFindParam: self.isDeliveryAmountFindParam,
            IsMaximumMonthsStockFindParam: self.isMaximumMonthsStockFindParam,
            IsExchangeTypeFindParam: self.isExchangeTypeFindParam,
            IsMinimumQuantityDeliveryFindParam: self.isMinimumQuantityDeliveryFindParam,
            UserFullName: self.currentUserFullName,
            ExpressCalc: true
        };

        ////////////////////////////////////////////////////////////////

        //Cargamos los combos
        loadCustomerList();
        loadSalesChannelsUser();
        loadGlobalvariables();

        resetValues();

        function resetValues() {

            self.showCosteo(false);

            self.price("0");
            self.priceOriginal("0");
            self.customerTotalCost("0");

            self.costoProducto_PorTON("0");
            self.typeChange("0");
            self.flete_PorITEM("0");
            self.flete_PorTON("0");
            self.almacenamientoCosto_PorITEM("0");
            self.almacenamientoCosto_PorTON("0");
            self.inout_PorITEM("0");
            self.inout_PorTON("0");
            self.merma_PorITEM("0");
            self.merma_PorTON("0");
            self.costoFinancieroMensual_PorITEM("0");
            self.costoFinancieroMensual_PorTON("0");

            self.exchangeInsurance("0");
            self.exchangeInsurance_PorTON("0");


            self.costoFin_PorTON("0");
            self.gastosFijos_PorITEM("0");
            self.gastosFijos_PorTON("0");
            self.impuestoDC_PorITEM("0");
            self.impuestoDC_PorTON("0");
            self.iibbAlicuota_PorITEM("0");
            self.iibbAlicuota_PorTON("0");
            self.comisiones_PorITEM("0");
            self.comisiones_PorTON("0");
            self.margenNetoUSD_PorTON("0");
            self.margenNetoOriginalUSD_PorTON("0");
            self.margenNetoPorc_PorTON("0");
            self.margenNetoOriginalPorc_PorTON("0");
            self.margenNetoEntidadOrigen("0");
            self.factorRofex("0");
            self.valorRofex("0");
            self.tiempoMedioStockMeses("0");
            self.leyendaCalculoCostoTransporte("");
            self.message("");
            self.observations("");
            
            self.gv_CostoFinancieroMensual("0");
            self.gv_CostoAlmacenamientoMensual("0");
            self.gv_FactorCostoAlmacenamientoMensual("0");
            self.gv_DiasStockPromedioDistLocal("0");
            self.freightType("0");
            self.precioVentaRofex("0");
            self.margenNetoUSDRofex("0");
            self.margenNetoPORCRofex("0");
            self.costoFinancieroMensual_PorITEM_Formula("");
            self.workingCapital_Formula("");
            self.tiempoMedioStockDias_Formula("");

            self.hasWayOfException("");
            self.exceptionApplyType("");
            self.wayOfExceptionValue("");

            self.isSaleModalityFindParam("");
            self.isGeographicAreaFindParam("");
            self.isPaymentDeadlineFindParam("");
            self.isQuantityOpenPurchaseOrderFindParam("");
            self.isDeliveryAmountFindParam("");
            self.isMaximumMonthsStockFindParam("");
            self.isExchangeTypeFindParam("");
            self.isMinimumQuantityDeliveryFindParam("");
        }

        function setValues(data) {
            self.price(data.Price);
            self.priceOriginal(data.PriceOriginal);
            self.customerTotalCost(data.CustomerTotalCost);

            self.costoProducto_PorTON(data.CostoProducto_PorTON.toFixed(5));
            self.typeChange(data.TypeChange.toFixed(3));
            self.flete_PorITEM(data.Flete_PorITEM.toFixed(2));
            self.flete_PorTON(data.Flete_PorTON);
            self.almacenamientoCosto_PorITEM(data.AlmacenamientoCosto_PorITEM.toFixed(0));
            self.almacenamientoCosto_PorTON(data.AlmacenamientoCosto_PorTON);
            self.inout_PorITEM(data.Inout_PorITEM);
            self.inout_PorTON(data.Inout_PorTON);
            self.merma_PorITEM(data.Merma_PorITEM.toFixed(2));
            self.merma_PorTON(data.Merma_PorTON);
            self.costoFinancieroMensual_PorITEM(data.CostoFinancieroMensual_PorITEM);
            self.costoFinancieroMensual_PorTON(data.CostoFinancieroMensual_PorTON);

            self.exchangeInsurance(data.ExchangeInsurance);
            self.exchangeInsurance_PorTON(data.ExchangeInsurance_PorTON);


            self.costoFin_PorTON(data.CostoFin_PorTON);
            self.gastosFijos_PorITEM(data.GastosFijos_PorITEM.toFixed(2));
            self.gastosFijos_PorTON(data.GastosFijos_PorTON);
            self.impuestoDC_PorITEM(data.ImpuestoDC_PorITEM.toFixed(2));
            self.impuestoDC_PorTON(data.ImpuestoDC_PorTON);
            self.iibbAlicuota_PorITEM(data.IIBBAlicuota_PorITEM.toFixed(2));
            self.iibbAlicuota_PorTON(data.IIBBAlicuota_PorTON);
            self.comisiones_PorITEM(data.Comisiones_PorITEM.toFixed(2));
            self.comisiones_PorTON(data.Comisiones_PorTON);
            self.margenNetoUSD_PorTON(data.MargenNetoUSD_PorTON);
            self.margenNetoOriginalUSD_PorTON(data.MargenNetoOriginalUSD_PorTON);
            self.margenNetoPorc_PorTON(data.MargenNetoPorc_PorTON);
            self.margenNetoOriginalPorc_PorTON(data.MargenNetoOriginalPorc_PorTON.toFixed(2));
            self.margenNetoEntidadOrigen(data.MargenNetoEntidadOrigen);
            self.factorRofex(data.FactorRofex.toFixed(4));
            self.valorRofex(data.ValorRofex.toFixed(3));
            self.tiempoMedioStockMeses(data.TiempoMedioStockMeses);
            self.leyendaCalculoCostoTransporte(data.LeyendaCalculoCostoTransporte);
            self.message(data.Message);
            self.observations(data.Observations);
            
            self.gv_CostoFinancieroMensual(data.GV_CostoFinancieroMensual.toFixed(2));
            self.gv_CostoAlmacenamientoMensual(data.GV_CostoAlmacenamientoMensual);
            self.gv_FactorCostoAlmacenamientoMensual(data.GV_FactorCostoAlmacenamientoMensual.toFixed(0));
            self.gv_DiasStockPromedioDistLocal(data.GV_DiasStockPromedioDistLocal);
            self.freightType(data.FreightType);
            self.precioVentaRofex(data.PrecioVentaRofex);
            self.margenNetoUSDRofex(data.MargenNetoUSDRofex);
            self.margenNetoPORCRofex(data.MargenNetoPORCRofex.toFixed(2));
            self.workingCapital(data.WorkingCapital.toFixed(2));

            self.costoFinancieroMensual_PorITEM_Formula(data.CostoFinancieroMensual_PorITEM_Formula);
            self.workingCapital_Formula(data.WorkingCapital_Formula);
            self.tiempoMedioStockDias_Formula(data.TiempoMedioStockDias_Formula);

            self.hasWayOfException(data.HasWayOfException);
            self.exceptionApplyType(data.ExceptionApplyType);
            self.wayOfExceptionValue(data.WayOfExceptionValue);

            self.isSaleModalityFindParam(data.IsSaleModalityFindParam);
            self.isGeographicAreaFindParam(data.IsGeographicAreaFindParam);
            self.isPaymentDeadlineFindParam(data.IsPaymentDeadlineFindParam);
            self.isQuantityOpenPurchaseOrderFindParam(data.IsQuantityOpenPurchaseOrderFindParam);
            self.isDeliveryAmountFindParam(data.IsDeliveryAmountFindParam);
            self.isMaximumMonthsStockFindParam(data.IsMaximumMonthsStockFindParam);
            self.isExchangeTypeFindParam(data.IsExchangeTypeFindParam);
            self.isMinimumQuantityDeliveryFindParam(data.IsMinimumQuantityDeliveryFindParam);
        }

        self.exceptionParamTypeDesc = function (val) {

            if (val == 1)
                return 'Parametro Variable';
            if (val == 2)
                return 'Parametro Encontrado';
            if (val == 3)
                return 'Parametro No Encontrado';
        }

        self.tiempoMedioStockDias = ko.computed(function () {
            return (self.tiempoMedioStockMeses() * 30);
        });
        self.maximoTiempoStockDias = ko.computed(function () {
            return (self.maximumMonthsStock() * 30);
        });        
        self.inout_USD = ko.computed(function () {
            return (self.inout_PorITEM() / self.typeChange()).toFixed(2);
        });
        self.inout_USDKg = ko.computed(function () {
            return (self.inout_PorITEM() / self.typeChange() / 1000).toFixed(4);
        });
        self.almacenamientoARS = ko.computed(function () {
            return (self.gv_CostoAlmacenamientoMensual() * self.tiempoMedioStockDias() / 30);
        });
        self.almacenamientoUSD = ko.computed(function () {
            return ((self.gv_CostoAlmacenamientoMensual() * self.tiempoMedioStockDias() / 30) / self.typeChange() ).toFixed(2);
        });
        self.almacenamientoUSDKg = ko.computed(function () {
            return (((self.gv_CostoAlmacenamientoMensual() * self.tiempoMedioStockDias() / 30) / self.typeChange()) / 1000).toFixed(4);
        });
        self.flete_PorITEMUSD = ko.computed(function () {
            return (self.flete_PorITEM() / self.typeChange()).toFixed(2);
        });
        self.price_View = ko.computed(function () {
            if (self.price() === '')
                return '0.00';
            else
                return parseFloat(self.price()).toFixed(3);
        });
        self.price_PorEntrega_View = ko.computed(function () {
            if (self.price() === '' || !self.price_PorEntrega())
                return '0.00';
            else
                return parseFloat(self.price_PorEntrega()).toFixed(2);
        });
        self.price_PorOC_View = ko.computed(function () {
            return parseFloat(self.price_PorOC()).toFixed(2);
        });

        self.flete_PorTON_View = ko.computed(function () {
            return parseFloat(self.flete_PorTON()).toFixed(5);
        });
        self.almacenamientoCosto_PorTON_View = ko.computed(function () {
            return parseFloat(self.almacenamientoCosto_PorTON()).toFixed(5);
        });
        self.inout_PorITEM_View = ko.computed(function () {
            return parseFloat(self.inout_PorITEM()).toFixed(0);
        });
        self.inout_PorTON_View = ko.computed(function () {
            return parseFloat(self.inout_PorTON()).toFixed(5);
        });
        self.merma_PorTON_View = ko.computed(function () {
            return parseFloat(self.merma_PorTON()).toFixed(5);
        });
        self.costoFinancieroMensual_PorTON_View = ko.computed(function () {
            return parseFloat(self.costoFinancieroMensual_PorTON()).toFixed(5);
        });
        self.gastosFijos_PorTON_View = ko.computed(function () {
            return parseFloat(self.gastosFijos_PorTON()).toFixed(5);
        });
        self.impuestoDC_PorTON_View = ko.computed(function () {
            return parseFloat(self.impuestoDC_PorTON()).toFixed(5);
        });
        self.iibbAlicuota_PorTON_View = ko.computed(function () {
            return parseFloat(self.iibbAlicuota_PorTON()).toFixed(5);
        });
        self.comisiones_PorTON_View = ko.computed(function () {
            return parseFloat(self.comisiones_PorTON()).toFixed(5);
        });
        self.customerTotalCost_View = ko.computed(function () {
            return parseFloat(self.customerTotalCost()).toFixed(5);
        });
        self.priceOriginal_View = ko.computed(function () {
            return parseFloat(self.priceOriginal()).toFixed(5);
        });
        self.margenNetoOriginalUSD_PorTON_View = ko.computed(function () {
            return parseFloat(self.margenNetoOriginalUSD_PorTON()).toFixed(5);
        });
        self.precioVentaRofex_View = ko.computed(function () {
            return parseFloat(self.precioVentaRofex()).toFixed(5);
        });

        //ROI
        self.margenBrutoPORCRofex = ko.computed(function () {
            return (parseFloat(self.costoFinancieroMensual_PorITEM()) + parseFloat(self.margenNetoPORCRofex())).toFixed(2);
        });

        self.workingCapitalRotation = ko.computed(function () {
            if (self.workingCapital() != 0)
            {
                return parseFloat(360 / self.workingCapital()).toFixed(2);
            }
            return 0;
        });

        self.workingCapitalROI = ko.computed(function () {
            return (parseFloat(self.margenBrutoPORCRofex()) * parseFloat(self.workingCapitalRotation())).toFixed(2);
        });

        self.productBuyAndSellDirect_View = ko.computed(function () {
            if (self.productBuyAndSellDirect() === true)
                return 'SI';
            else
                return 'NO';
        });

        self.productInOutStorage_View = ko.computed(function () {
            if (self.productInOutStorage() === true)
                return 'SI';
            else
                return 'NO';
        });

        function loadCustomerList() {
            //url: "/GetCustomersSpotByUser/" + self.currentCustomerID(),
            $.ajax({
                url: "/GetCustomersByUser/" + self.currentCustomerID(),
                type: "GET",
                success: function (resp) {
                    self.customersList(resp);
                    if (self.isSellerUser()) {
                        self.customerCompanyNameValidateTrue(false);
                        var itemCustSelect = self.customersList()[0];
                        self.selectedCustomer(itemCustSelect);
                        self.customerContactName("Contacto Cliente");
                        self.customerCompanyName("Empresa Cliente");
                    }
                    else {
                        self.customerCompanyNameValidateTrue(true);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                                    alert("Ha ocurrido un error, por favor intente mas tarde");
                                    console.log(xhr);
                                    console.log(ajaxOptions);
                                    console.log(thrownError);
                        }
            });
        }

        function loadSalesChannelsUser() {
            $.ajax({
                url: "/GetSalesChannelsByUser/" + self.userId(),
                type: "GET",
                success: function (resp) {
                    self.salesChannelsUserList(resp);
                    console.log("-- GetSalesChannelsByUser --");
                    console.log(ko.toJSON(resp));
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        function loadGlobalvariables() {
            $.ajax({
                url: "/GetGlobalVariables",
                type: "GET",
                success: function (resp) {
                    //console.log("GET GLOBAL VARIABLES");
                    //console.log(ko.toJSON(resp));
                    self.globalvariables(resp);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        self.selectedCustomer.subscribe(function (item) {
            console.log(ko.toJSON(item));
            if (item !== undefined) {
                self.customerID(item.CustomerID);
                self.customerCreditRatingID(item.CreditRatingID);
                self.customerIsSpot(item.IsSpot);
                self.customerContactName(item.ContactName);
                self.customerCompanyName(item.Company);
                self.customerDelayAverageDays(item.DelayAverageDays);
                if (item.IsSpot)
                {
                    self.customerContactName("Contacto Cliente");
                    self.customerCompanyName("Empresa Cliente");
                }
            }
            else {
                self.customerID("");
                self.customerCreditRatingID("");
                self.customerIsSpot(false);
                self.customerContactName("");
                self.customerCompanyName("");
                self.customerDelayAverageDays("");
            }
            console.log("loadProductList");
            loadProductList();
            loadPaymentDeadlineList(self.customerCreditRatingID());
        });

        function loadProductList() {
            if (self.customerID() === undefined || self.customerID() === "")
            {
                console.log("Error en loadProductList() valor de self.customerID() indefinido");
                self.productsList("");
                return;
            }

            $.ajax({
                url: "/GetProductsDistribucionLocalActive/" + self.customerID(),
                type: "GET",
                success: function (resp) { self.productsList(resp); },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        self.selectedProductID.subscribe(function (productID) {
            
            if (self.customerID() === undefined || self.customerID() === "") {
                console.log("Error en selectedProductID.subscribe() valor de self.customerID() indefinido");
                return;
            }
            if (productID !== undefined) {
                var _seeCosting = 0;
                if (self.currentUserIsSellerUser() == "value")
                    _seeCosting = 1;

                //$.ajax({
                //    url: "/GetSalesModalityByProduct/" + productID,
                //    type: "GET",
                //    success: function (resp) { self.saleModalityList(resp); },
                //    error: function (xhr, ajaxOptions, thrownError) {
                //        alert("Ha ocurrido un error, por favor intente mas tarde");
                //        console.log("ERROR EN: GetSalesModalityByProduct()");
                //        console.log(xhr);
                //        console.log(ajaxOptions);
                //        console.log(thrownError);
                //    }
                //});
                $.ajax({
                    url: "/GetShowCosteoByProduct/" + self.customerID() + "/" + productID + "/" + ((self.currentUserSeeCosting() == "value") ? 1 : 0),
                    type: "GET",
                    success: function (resp) {
                        self.canShowCosteo(resp);
                        //console.log(ko.toJSON(self));
                        //console.log(ko.toJSON(self.minimumQuantityDelivery()));
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Ha ocurrido un error, por favor intente mas tarde");
                        console.log("ERROR EN: GetShowCosteoByProduct()");
                        console.log(xhr);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }
                });
                $.ajax({
                    url: "/GetProductDetails/" + productID,
                    type: "GET",
                    success: function (resp) {
                        //console.log("PRODUCTO");
                        //console.log(ko.toJSON(resp));
                        self.product(resp);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Ha ocurrido un error, por favor intente mas tarde");
                        self.product("");
                        console.log("ERROR EN: GetProductDetails()");
                        console.log(xhr);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }

                });
                //console.log(ko.toJSON(self.minimumQuantityDelivery()));
            }
            else {
                self.canShowCosteo(false);
                //self.saleModalityList.removeAll();
                self.selectedProductID(undefined);
                self.product("");
            }

            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });

        //ojoo
        self.maximumMonthsStock.subscribe(function (quantity) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });

        self.quantityOpenPurchaseOrder.subscribe(function (quantity) {
            if (self.deliveryAmount() !== undefined && self.deliveryAmount() !== 0) {
                self.minimumQuantityDelivery(self.quantityOpenPurchaseOrder() / self.deliveryAmount());
            }
            else {
                self.minimumQuantityDelivery(quantity);//OJOOO
            }
            //alert(self.minimumQuantityDelivery());
            console.log(ko.toJSON(self.minimumQuantityDelivery()));
            //console.log(ko.toJSON(quantity));
            

            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });

        self.minimumQuantityDelivery.subscribe(function (quantity) {
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);
        });

        //function loadGeographicAreaList() {
        //    $.ajax({
        //        url: "/GetGeographicAreas",
        //        type: "GET",
        //        success: function (resp) { self.geographicAreaList(resp); },
        //        error: function (xhr, ajaxOptions, thrownError) {
        //        alert("Ha ocurrido un error, por favor intente mas tarde");
        //        console.log(xhr);
        //        console.log(ajaxOptions);
        //        console.log(thrownError);
        //    }
                
        //    });
        //}

        function loadPaymentDeadlineList(creditRatingID) {
            if (creditRatingID !== undefined && creditRatingID > 0) {
                $.ajax({
                    url: "/GetPaymentDeadlineByCreditRating/" + creditRatingID,
                    type: "GET",
                    success: function (resp) { self.paymentDeadlineList(resp); },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Ha ocurrido un error, por favor intente mas tarde");
                        console.log(xhr);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }
                });
            }
            else {
            }
        }

        //function loadExchangeTypeList() {
        //    $.ajax({
        //        url: "/GetExchangeType",
        //        type: "GET",
        //        success: function (resp) { self.exchangeTypeList(resp); },
        //        error: function (xhr, ajaxOptions, thrownError) {
        //                            alert("Ha ocurrido un error, por favor intente mas tarde");
        //                            console.log(xhr);
        //                            console.log(ajaxOptions);
        //                            console.log(thrownError);
        //                }
                
        //    });
        //}
        
        //function loadMaximumMonthsStockList() {
        //    $.ajax({
        //        url: "/GetMaximumMonthsStock",
        //        type: "GET",
        //        success: function (resp) { self.maximumMonthsStockList(resp); },
        //        error: function (xhr, ajaxOptions, thrownError) {
        //        alert("Ha ocurrido un error, por favor intente mas tarde");
        //        console.log(xhr);
        //        console.log(ajaxOptions);
        //        console.log(thrownError);
        //    }
                
        //    });
        //}

        //function loadDeliveryAmountList() {
        //    $.ajax({
        //        url: "/GetDeliveryAmount",
        //        type: "GET",
        //        success: function (resp) {
        //            self.deliveryAmountList(resp);
        //        },
        //        error: function (xhr, ajaxOptions, thrownError) {
        //        alert("Ha ocurrido un error, por favor intente mas tarde");
        //        console.log(xhr);
        //        console.log(ajaxOptions);
        //        console.log(thrownError);
        //    }
                
        //    });
        //}

        //self.selectedSaleModalityID.subscribe(function (saleModalityID) {
        //    if (saleModalityID !== undefined) {
        //        //$.ajax({
        //        //    url: "/GetGeographicAreasBySaleModality/" + saleModalityID,
        //        //    type: "GET",
        //        //    success: function (resp) {
                        
        //        //        self.geographicAreaList(resp);
        //        //    },
        //        //    error: function (xhr, ajaxOptions, thrownError) {
        //        //        alert("Ha ocurrido un error, por favor intente mas tarde");
        //        //        console.log(xhr);
        //        //        console.log(ajaxOptions);
        //        //        console.log(thrownError);
        //        //    }
        //        //});
        //        //$.ajax({
        //        //    url: "/GetDeliveryAmountsBySaleModality/" + saleModalityID,
        //        //    type: "GET",
        //        //    success: function (resp) {
        //        //        self.deliveryAmountList(resp);
        //        //    },
        //        //    error: function (xhr, ajaxOptions, thrownError) {
        //        //        alert("Ha ocurrido un error, por favor intente mas tarde");
        //        //        console.log(xhr);
        //        //        console.log(ajaxOptions);
        //        //        console.log(thrownError);
        //        //    }
        //        //});
        //        //$.ajax({
        //        //    url: "/GetMaximumMonthsStockBySaleModality/" + saleModalityID,
        //        //    type: "GET",
        //        //    success: function (resp) { self.maximumMonthsStockList(resp); },
        //        //    error: function (xhr, ajaxOptions, thrownError) {
        //        //        alert("Ha ocurrido un error, por favor intente mas tarde");
        //        //        console.log(xhr);
        //        //        console.log(ajaxOptions);
        //        //        console.log(thrownError);
        //        //    }
        //        //});
        //        //$.ajax({
        //        //    url: "/GetExchangeTypeBySaleModality/" + saleModalityID,
        //        //    type: "GET",
        //        //    success: function (resp) { self.exchangeTypeList(resp); },
        //        //    error: function (xhr, ajaxOptions, thrownError) {
        //        //        alert("Ha ocurrido un error, por favor intente mas tarde");
        //        //        console.log(xhr);
        //        //        console.log(ajaxOptions);
        //        //        console.log(thrownError);
        //        //    }
        //        //});
        //        //$.ajax({
        //        //    url: "/GetSaleModalityDetails/" + saleModalityID,
        //        //    type: "GET",
        //        //    success: function (resp) {
        //        //        self.saleModality(resp);
        //        //    },
        //        //    error: function (xhr, ajaxOptions, thrownError) {
        //        //        alert("Ha ocurrido un error, por favor intente mas tarde");
        //        //        console.log(xhr);
        //        //        console.log(ajaxOptions);
        //        //        console.log(thrownError);
        //        //    }
        //        //});
        //    }
        //    resetValues();
        //    self.canCalculateProductQuote(true);
        //    self.canCreateProductQuote(false);
        //});

 
        //self.selectedGeographicArea.subscribe(function (item) {

        //    if (item !== undefined) {
        //        self.geographicAreaID(item.GeographicAreaID);
        //        self.geographicAreaName(item.Name);
        //        }
        //        else {
        //        self.geographicAreaID("");
        //        self.geographicAreaName("");
        //        }

        //    resetValues();
        //    self.canCalculateProductQuote(true);
        //    self.canCreateProductQuote(false);

        //});

        self.selectedPaymentDeadline.subscribe(function (item) {
            //alert(ko.toJSON(item));
            if (item !== undefined) {
                self.paymentDeadlineID(item.PaymentDeadlineID);
                self.paymentDeadlineName(item.Description);
                self.paymentDeadlineMonths(item.Months);
                self.paymentDeadlineDays(item.Days);
            }
            else {
                self.paymentDeadlineID("");
                self.paymentDeadlineName("");
                self.paymentDeadlineMonths("");
                self.paymentDeadlineDays("");
            }

            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);

        });

        //self.selectedExchangeType.subscribe(function (item) {
        //    //alert(ko.toJSON(item));
        //    if (item !== undefined) {
        //        self.exchangeTypeID(item.ExchangeTypeID);
        //        self.exchangeTypeName(item.Description);
        //        self.exchangeTypeLargeDescription(item.LargeDescription);
        //    }
        //    else {
        //        self.exchangeTypeID("");
        //        self.exchangeTypeName("");
        //        self.exchangeTypeLargeDescription("");
        //    }

        //    resetValues();
        //    self.canCalculateProductQuote(true);
        //    self.canCreateProductQuote(false);

        //});

        //self.selectedMaximumMonthsStock.subscribe(function (item) {
        //    if (item !== undefined) {
        //        self.maximumMonthsStock(item.StockTimeID);
        //    }
        //    else {
        //        self.maximumMonthsStock(0);
        //    }

        //    resetValues();
        //    self.canCalculateProductQuote(true);
        //    self.canCreateProductQuote(false);

        //});

        //self.selectedDeliveryAmount.subscribe(function (item) {
        //    if (item !== undefined) {

        //        self.deliveryAmount(item.DeliveryAmountID);

        //        self.minimumQuantityDelivery(self.quantityOpenPurchaseOrder() / item.DeliveryAmountID);
        //    }
        //    else {
        //        self.minimumQuantityDelivery("");
        //        self.deliveryAmount(0);
        //    }
             
        //    resetValues();
        //    self.canCalculateProductQuote(true);
        //    self.canCreateProductQuote(false);

        //});

        function isValidProductQuoteInput()
        {
            if (self.minimumQuantityDelivery() === 0) {
                alert('La Cantidad Minima por Entrega (Kg) calculada a partir de la Cantidad Total Orden de Compra (Kg) y la Cantidad de Entregas no es válida.');
                return false;
            }
            else
            {
                return true;
            }                
        }

        self.resetGetGlobalVariables = function () {
            loadGlobalvariables();
        }

        //BOTON CALCULAR
        self.calculateProductQuote = function () {
            
            resetValues();
            self.canCalculateProductQuote(true);
            self.canCreateProductQuote(false);

            self.customerCompanyNameValidateTrue(true);

            //alert(productQuote.userObservations);

            console.log("PRODUCT QUOTE - ANTES");
            console.log(ko.toJSON(productQuote));

            if (isValidProductQuoteInput() === false) {
                return;
            }
                
            if (self.errors().length > 0) {
                self.errors.showAllMessages();
                return;
            }

            console.log("PRODUCT QUOTE - NO TIENE ERRORES");

            $.ajax({
                url: "/CalculateProductQuote",
                type: "POST",
                data: ko.toJSON(productQuote),
                contentType: "application/json",
                success: function (data) {
                    console.log("PRODUCT QUOTE - DESPUES");
                    console.log(ko.toJSON(data));

                    if (data.Message === "")
                    {
                        setValues(data);
                        self.canCalculateProductQuote(false);
                        self.canCreateProductQuote(true);
                    }
                    else
                    {
                        //alert(data.Message);
                        resetValues();
                        self.message(data.Message);
                    }
                },
                error: function () {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    resetValues();
                }
            });

            self.showCosteo(true);
        };

        //BOTON CREAR
        self.createProductQuote = function () {
            //alert(ko.toJSON(productQuote));
            console.log("PRODUCT QUOTE EXPRESS CREATE - ENVIO DATOS");
            console.log(ko.toJSON(productQuote));
            if (self.errors().length === 0) { 
                $.ajax({
                    url: "/CreateProductQuoteExpress",
                    type: "POST",
                    data: ko.toJSON(productQuote),
                    contentType: "application/json",
                    success: function (data) {
                        //alert(ko.toJSON(data));
                        //alert(data.returnUrl);
                        window.location = data.returnUrl;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Ha ocurrido un error, por favor intente mas tarde");
                        resetValues();
                        console.log(xhr);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }
                });
            }
            else {
                alert('Ha ocurrido un error, por favor intente mas tarde');
                alert(ko.toJSON(self.errors));
                self.errors.showAllMessages();
            }
        };

        self.generateException = function () {
            console.log("LLAMADO A GENERAR EXCEPCION");
            console.log(ko.toJSON(productQuote));
            if (self.errors().length === 0) {
                $.ajax({
                    url: "/WayOfException/CreateWhitDefault",
                    type: "POST",
                    data: ko.toJSON(productQuote),
                    contentType: "application/json",
                    success: function () {
                        //window.location = "/WayOfException/CreateWhitDefault";
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Ha ocurrido un error, por favor intente mas tarde");
                        resetValues();
                        console.log(xhr);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }
                });
            }
            else {
                alert('Ha ocurrido un error, por favor intente mas tarde');
                alert(ko.toJSON(self.errors));
                self.errors.showAllMessages();
            }
        }

    };
    var vm = new ProductQuoteExpressViewModel();

    //Con esta linea se logra que los mensajes de validacion funcionen con el custom binding de commaDecimalFormatter, porque por defecto solo anda con 'value'
    ko.validation.makeBindingHandlerValidatable('commaDecimalFormatter');
    vm.errors = ko.validation.group(vm);

    ko.applyBindings(vm);

})();

//Binding handlers para revisar
//https://www.moonlightbytes.com/blog/useful-knockout-js-binding-handlers