(function() {
  var App;

  App = Ember.Application.create();

  App.Fee = Ember.Object.extend({
    Id: null,
    Name: null,
    Type: null,
    Amount: null
  });

  App.Fee.reopenClass({
    loadAll: function() {
      return $.getJSON("fees").then(function(response) {
        var fees;
        fees = Em.A();
        response.forEach(function(child) {
          return fees.pushObject(App.Fee.create(child));
        });
        return fees;
      });
    },
    save: function(fee) {
      return $.ajax({
        type: 'post',
        url: 'fees/add',
        data: {
          Id: fee.Id,
          Name: fee.Name,
          Type: fee.Type,
          Amount: fee.Amount
        },
        async: false
      });
    }
  });

  App.FeesRoute = Ember.Route.extend({
    model: function() {
      return App.Fee.loadAll();
    }
  });

  App.FeesAddRoute = Ember.Route.extend({
    setupController: function(controller) {}
  });

  App.FeesEditRoute = Ember.Route.extend({
    setupController: function(controller) {}
  });

  App.FeesController = Ember.ObjectController.extend({
    add: function() {
      var _this = this;
      return $.getJSON("fees/add").then(function(newFee) {
        _this.get('controllers.fees').pushObject(newFee);
        return _this.transitionToRoute('fees');
      });
    },
    edit: function() {}
  });

  App.Router.map(function() {
    this.resource('families');
    this.resource('fees', function() {
      return this.route('add', this.route('edit', {
        path: ":Id"
      }));
    });
    this.resource('payments');
    this.resource('discounts');
    this.resource('ledger');
    this.resource('reports');
    this.resource('invoicing');
    return this.resource('tools');
  });

}).call(this);
