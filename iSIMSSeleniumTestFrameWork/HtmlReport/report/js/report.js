(function () {
    var root = this;
    var str = {
        truncate: function (string, length, truncation) {
            length = length || 30;
            truncation = _.isUndefined(truncation) ? '...' : truncation;
            return string.length > length ?
                string.slice(0, length - truncation.length) + truncation : String(string);
        }

    };

    str.parseQuery = str.toQueryParams;
    root._.mixin(str);
}());

console.log(result);
var raw = Result;
var statusToIconMapping = {
    0: "glyphicon glyphicon-ok",
    1: "glyphicon glyphicon-remove",
    2: "glyphicon glyphicon-warning-sign"
}


var formatTime = function (timemillis) {
    var total = timemillis;
    if (total < 1000) {
        return total + " ms";
    }
    if (total < 60000) {
        var sec = Math.round(total / 1000);
        return sec + " seconds";
    }
    if (total < 3600000) {
        var min = Math.round(total / 60000);
        var sec = Math.round((total - min * 60000) / 1000);
        return min + " min " + sec + " seconds";
    }
    return "TODO parse ms:" + total;
}
var Result = Backbone.Model.extend({
    getBrowser: function () {
        if (this.get('Metadata')) {
            return this.get('Metadata')['browser'];
        }
        return null;
    },

    getMethodName: function () {
        var parameters = this.get('Parameters');
        if (parameters === null) {
            parameters = "";
        }
        return this.get('MethodName') + "(" + parameters + ")";
    },
    getError: function (truncate) {
        if (this.get('Error')) {
            var raw = this.get('Error')['Message'];
            var trunc = _(this.get('Error')['Message']).truncate(30);
            if (truncate) {
                return this.get('Error')['Type'] + " : " + _.escape(trunc);
            } else {
                return this.get('Error')['Type'] + " : " + _.escape(raw);
            }
        } else {
            return null;
        }
    },

    getGlyphIcon: function () {
        return statusToIconMapping[this.get('Status')];
    },
    getDuration: function () {
        var total = this.get('TimeMillis');
        return formatTime(total);
    },

    getLogByType: function (type) {
        var res = [];
        for (var i = 0; i < this.get('Logs').length; i++) {
            var log = this.get("Logs")[i];
            if (log.Type === type) {
                res.push(log);
            }
        }
        return res;
    },
    getScreenshots: function () {
        return this.getLogByType("Screenshot");
    },

    getLogMessages: function () {
        return this.getLogByType("Log");
    }



});


var Results = Backbone.Collection.extend({
    model: Result,
    getResultById: function (id) {
        if (!id) {
            return null;
        }
        for (var i = 0; i < this.models.length; i++) {
            var res = this.models[i];
            if (res.get('Uuid') === id) {
                return res;
            }
        }
        return null;
    }
});

var HeaderView = Backbone.View.extend({
    el: '#header-title',
    render: function () {
        var template = _.template($('#header-title-template').html());
        var res = template({
            title: result.Name + " (" + formatTime(result.TimeMillis) + ")"
        });
        this.$el.html(res);
    }
});


var SummaryView = Backbone.View.extend({
    el: '#summary',
    render: function () {
        var template = _.template($('#result-summary-template').html());
        var passed = result.PassedTests.length;
        var failed = result.FailedTests.length;
        var skipped = result.SkippedTests.length;
        var total = passed + skipped + failed;

        var res = template({
            total: total,
            passed: passed,
            skipped: skipped,
            failed: failed
        });
        this.$el.html(res);
    }
});

var DetailView = Backbone.View.extend({
    // doesn't exist yet.
    //el: '#detail',
    render: function (result) {
        var template = _.template($('#result-detail-template').html());
        var res = template({ result: result });
        console.log("res " + res);
        $("#detail").html(res);
        this.delegateEvents();
    }
});
var ResultView = Backbone.View.extend({
    initialize: function () {
        this.inner = new DetailView();
        this.summary = new SummaryView();
        this.header = new HeaderView();
    },
    el: '#result-container',
    render: function (status, id) {
        this.status = status;
        var rs = new Results();
        if (status === "all") {
            rs.add(result.PassedTests);
            rs.add(result.SkippedTests);
            rs.add(result.FailedTests);
        }
        if (status === "skipped") {
            rs.add(result.SkippedTests);
        }

        if (status === "passed") {
            rs.add(result.PassedTests);
        }
        if (status === "failed") {
            rs.add(result.FailedTests);
        }
        var template = _.template($('#result-list-template').html());
        var res = template({ results: rs.models, expand: id });
        this.$el.html(res);


        var selected = rs.getResultById(id);
        this.inner.render(selected);
        this.summary.render();
        this.header.render();

        $(".left-menu").removeClass("active");
        $("#left-menu-" + status).addClass("active");
    },
    events: {
        'click .result-row': 'expand',
        'keyup :input': 'logKey',
        'keypress :input': 'logKey'
    },
    logKey: function (ev) {
        console.log(ev);
    },
    expand: function (ev) {
        var tr = ev.currentTarget;
        router.navigate('show/' + this.status + '/id/' + tr.id, { trigger: true });
    }
});




var Router = Backbone.Router.extend({
    routes: {
        '': 'home',
        'show/:status(/id/:id)': 'filter'
    }
});

var resultView = new ResultView();
var router = new Router();

router.on('route:home', function () {
    resultView.render("all");
});
router.on('route:filter', function (data, id) {
    resultView.render(data, id);
});

Backbone.history.start();


$(document).ready(function () {


});
