var sfa = sfa || {};

sfa.settings = {
    init: function () {
        document.body.className = ((document.body.className) ? document.body.className + ' js-enabled' : 'js-enabled');
    }
};

sfa.tabs = {

    elems: {
        tabs: $('ul.js-tabs li a'),
        panels: $('.js-tab-pane')
    },

    init: function () {

        if (this.elems.tabs) {
            this.setUpEvents(this.elems.tabs);
            this.hidePanels(this.elems.panels);
        }

        this.elems.tabs.eq(0).click();

    },

    hidePanels: function (panels) {
        panels.hide();
    },

    showPanel: function (panel) {
        panel.show();
    },

    setUpEvents: function (tabs) {

        var that = this;

        tabs.on('click touchstart', function (e) {

            tabs.closest('li').removeClass('current');
            $(this).closest('li').addClass('current');

            var target = $(this).attr('href');

            that.hidePanels(that.elems.panels);
            that.showPanel($(target));

            e.preventDefault();
        });

    }
};

sfa.focusSwitch = {
    init: function () {

        var fields = $('.focus-switch');

        fields.on('keyup', function () {

            var that = $(this),
                length = that.val().length,
                maxlength = that.attr('maxlength'),
                nextid = that.data('next-id');

            console.log(nextid);

            if (length == maxlength) {
                $('#' + nextid).focus();
            }

        });
    }
};

sfa.settings.init();
sfa.tabs.init();

if ($('.focus-switch').length > 0) {
    sfa.focusSwitch.init();
}
