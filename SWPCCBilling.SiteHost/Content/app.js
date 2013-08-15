(function() {
  var App;

  App = Ember.Application.create();

  App.Fee = Ember.Object.extend({});

  App.Fee.reopenClass({
    loadAll: function() {
      return $.getJSON("fees").then(function(response) {
        var fees;
        fees = Em.A();
        response.data.children.forEach(function(child) {
          return fees.pushObject(App.Fee.create(child.data));
        });
        return fees;
      });
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
