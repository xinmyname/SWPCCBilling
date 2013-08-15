(function() {
  var App;

  App = Ember.Application.create();

  App.Fee = Ember.Object.extend({});

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
    }
  });

  App.FeesRoute = Ember.Route.extend({
    model: function() {
      return App.Fee.loadAll();
    }
  });

  App.Router.map(function() {
    this.resource('families');
    this.resource('fees');
    this.resource('payments');
    this.resource('discounts');
    this.resource('ledger');
    this.resource('reports');
    this.resource('invoicing');
    return this.resource('tools');
  });

}).call(this);
