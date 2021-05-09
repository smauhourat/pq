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

    var CustomerProductViewModel = function () {

        var self = this;

        self.customerID = ko.observable($('#myHDCustomerID').val());
        self.customerName = ko.observable($('#myHDCustomerName').val());

        self.assignedProductsList = ko.observableArray([]);
        self.availableProductsList = ko.observableArray([]);

        self.Query1 = ko.observable('');
        self.Query = ko.observable('');

        self.assignedProductsListFiltered = ko.computed(function () {
            var q = self.Query1();
            return self.assignedProductsList().filter(function (i) {
                return i.ProductName.toLowerCase().indexOf(q) >= 0;
            });
        });

        self.availableProductsListFiltered = ko.computed(function () {
            var q = self.Query();
            return self.availableProductsList().filter(function (i) {
                return i.FullName.toLowerCase().indexOf(q) >= 0;
            });
        });

        ko.bindingHandlers.click = {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel, context) {
                var accessor = valueAccessor();
                var clicks = 0;
                var timeout = 200;

                $(element).click(function (event) {
                    if (typeof (accessor) === 'object') {
                        var single = accessor.single;
                        var double = accessor.double;
                        clicks++;
                        if (clicks === 1) {
                            setTimeout(function () {
                                if (clicks === 1) {
                                    single.call(viewModel, context.$data, event);
                                } else {
                                    double.call(viewModel, context.$data, event);
                                }
                                clicks = 0;
                            }, timeout);
                        }
                    } else {
                        accessor.call(viewModel, context.$data, event);
                    }
                });
            }
        };

        loadAssignedProductsList(self.customerID());
        loadAvailableProductsList(self.customerID());

        function loadAssignedProductsList(customerID) {
            //alert(customerID);
            if (customerID !== undefined) {
                $.ajax({
                    url: "../GetAssignedProductsApi/" + customerID,
                    type: "GET",
                    success: function (resp) {
                        console.log(ko.toJSON(resp));
                        self.assignedProductsList(resp);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Ha ocurrido un error, por favor intente mas tarde");
                        console.log(xhr);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }

                    //error: function (err) { alert("Ha ocurrido un error, por favor intente mas tarde" + err.status); }
                });
            }
        }

        function loadAvailableProductsList(customerID) {
            //alert(customerID);
            if (customerID !== undefined) {
                $.ajax({
                    url: "../GetAvailableProductsApi/" + customerID,
                    type: "GET",
                    success: function (resp) {
                        console.log(ko.toJSON(resp));
                        self.availableProductsList(resp);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Ha ocurrido un error, por favor intente mas tarde");
                        console.log(xhr);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }

                    //error: function (err) { alert("Ha ocurrido un error, por favor intente mas tarde" + err.status); }
                });
            }
        }


        self.changeCustomerProductCalDetailsValue = function(customerProduct) {
            console.log(ko.toJSON(customerProduct));
            $.ajax({
                url: "../ChangeCustomerProductCalDetailsValue",
                type: "POST",
                data: ko.toJSON(customerProduct),
                contentType: "application/json",
                success: function () {
                    loadAssignedProductsList(customerProduct.CustomerID);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        self.assignProductToCustomer = function (product) {
            //alert(product.ProductID);
            console.log(ko.toJSON(product));
            $.ajax({
                url: "../AddProductToCustomer/" + self.customerID(),
                type: "POST",
                data: ko.toJSON(product),
                contentType: "application/json",
                success: function () {
                    loadAssignedProductsList(self.customerID());
                    loadAvailableProductsList(self.customerID());
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        

        //desassignProductToCustomer
        self.desassignProductToCustomer = function (customerProductID) {
            //alert(product.ProductID);
            //console.log(ko.toJSON(product));
            $.ajax({
                url: "../api/CustomerProductApi/" + customerProductID,
                type: "DELETE",
                success: function () {
                    loadAssignedProductsList(self.customerID());
                    loadAvailableProductsList(self.customerID());
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        self.deleteAllCustomerProductsByCustomer = function () {
            //console.log(ko.toJSON(product));
            //alert(1);
            $.ajax({
                url: "../DelAllCustomerProductsByCustomer/" + self.customerID(),
                type: "POST",
                //data: ko.toJSON(product),
                contentType: "application/json",
                success: function () {
                    loadAssignedProductsList(self.customerID());
                    loadAvailableProductsList(self.customerID());
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        self.addAllProductsToCustomer = function () {
            $.ajax({
                url: "../AddAllProductsToCustomer/" + self.customerID(),
                type: "POST",
                //data: ko.toJSON(product),
                contentType: "application/json",
                success: function () {
                    loadAssignedProductsList(self.customerID());
                    loadAvailableProductsList(self.customerID());
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

    };

        
    var vm = new CustomerProductViewModel();
    vm.errors = ko.validation.group(vm);

    ko.applyBindings(vm);

    //ko.applyBindings(new MSViewModel());

})();
