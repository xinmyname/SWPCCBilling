(function() {
  var App;

  App = Ember.Application.create();

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
