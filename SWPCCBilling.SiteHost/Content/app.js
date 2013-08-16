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
    setupController: function(controller) {
      return controller.set('fee', App.Fee.create());
    }
  });

  App.FeesController = Ember.ObjectController.extend({
    add: function() {
      var newFee;
      newFee = App.Fee.save(this.fee);
      this.get('controllers.fees').pushObject(newFee);
      return this.transitionToRoute('fees');
    }
  });

  App.Router.map(function() {
    this.resource('families');
    this.resource('fees', function() {
      return this.route('add');
    });
    this.resource('payments');
    this.resource('discounts');
    this.resource('ledger');
    this.resource('reports');
    this.resource('invoicing');
    return this.resource('tools');
  });

}).call(this);
