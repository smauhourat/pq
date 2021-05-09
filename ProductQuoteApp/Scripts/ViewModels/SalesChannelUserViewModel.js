(function () {

    ko.validation.init({
        insertMessages: true,
        messagesOnModified: true,
        decorateElement: true,
        errorElementClass: 'wrong-field'
    }, true);

    var SalesChannelUserViewModel = function () {

        var self = this;

        self.userID = ko.observable($('#myHDUserID').val());
        self.userName = ko.observable($('#myHDUserName').val());

        self.assignedSalesChannelsList = ko.observableArray([]);
        self.availableSalesChannelsList = ko.observableArray([]);

        self.Query1 = ko.observable('');
        self.Query = ko.observable('');

        self.assignedSalesChannelsListFiltered = ko.computed(function () {
            var q = self.Query1();
            return self.assignedSalesChannelsList().filter(function (i) {
                return i.SalesChannelName.toLowerCase().indexOf(q) >= 0;
            });
        });

        self.availableSalesChannelsListFiltered = ko.computed(function () {
            var q = self.Query();
            return self.availableSalesChannelsList().filter(function (i) {
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

        loadAssignedSalesChannelsList(self.userID());
        loadAvailableSalesChannelsList(self.userID());

        function loadAssignedSalesChannelsList(userID) {
            
            if (userID !== undefined) {
                $.ajax({
                    url: "../GetAssignedSalesChannels/" + userID,
                    type: "GET",
                    success: function (resp) {
                        //console.log(ko.toJSON(resp));
                        self.assignedSalesChannelsList(resp);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Ha ocurrido un error, por favor intente mas tarde");
                        console.log(xhr);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }
                });
            }
        }

        function loadAvailableSalesChannelsList(userID) {
            if (userID !== undefined) {
                $.ajax({
                    url: "../GetAvailableSalesChannels/" + userID,
                    type: "GET",
                    success: function (resp) {
                        console.log(ko.toJSON(resp));
                        self.availableSalesChannelsList(resp);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Ha ocurrido un error, por favor intente mas tarde");
                        console.log(xhr);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }
                });
            }
        }

        self.assignSalesChannelToUser = function (salesChannel) {
            //console.log(ko.toJSON(salesChannel));
            $.ajax({
                url: "../AddSalesChannelToUser/" + self.userID(),
                type: "POST",
                data: ko.toJSON(salesChannel),
                contentType: "application/json",
                success: function () {
                    loadAssignedSalesChannelsList(self.userID());
                    loadAvailableSalesChannelsList(self.userID());
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        self.desassignSalesChannelToUser = function (salesChannelUserID) {
            $.ajax({
                url: "../DelSalesChannelToUser/" + salesChannelUserID,
                type: "POST",
                success: function () {
                    loadAssignedSalesChannelsList(self.userID());
                    loadAvailableSalesChannelsList(self.userID());
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        self.deleteAllSalesChannelByUser = function () {
            $.ajax({
                url: "../DelAllSalesChannelByUser/" + self.userID(),
                type: "POST",
                //data: ko.toJSON(product),
                //contentType: "application/json",
                success: function () {
                    loadAssignedSalesChannelsList(self.userID());
                    loadAvailableSalesChannelsList(self.userID());
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Ha ocurrido un error, por favor intente mas tarde");
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            });
        }

        self.addAllSalesChannelsToUser = function () {
            $.ajax({
                url: "../AddAllSalesChannelsToUser/" + self.userID(),
                type: "POST",
                //data: ko.toJSON(product),
                //contentType: "application/json",
                success: function () {
                    loadAssignedSalesChannelsList(self.userID());
                    loadAvailableSalesChannelsList(self.userID());
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

        
    var vm = new SalesChannelUserViewModel();
    vm.errors = ko.validation.group(vm);

    ko.applyBindings(vm);

})();
