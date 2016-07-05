(function() {

    "use strict";

    var matched, browser;

    jQuery.uaMatch = function(ua) {
        ua = ua.toLowerCase();

        var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
            /(webkit)[ \/]([\w.]+)/.exec(ua) ||
            /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
            /(msie) ([\w.]+)/.exec(ua) ||
            ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) || [];

        return {
            browser: match[1] || "",
            version: match[2] || "0"
        };
    };

    matched = jQuery.uaMatch(navigator.userAgent);
    browser = {};

    if (matched.browser) {
        browser[matched.browser] = true;
        browser.version = matched.version;
    }

    // Chrome is Webkit, but Webkit is also Safari.
    if (browser.chrome) {
        browser.webkit = true;
    } else if (browser.webkit) {
        browser.safari = true;
    }

    jQuery.browser = browser;

    jQuery.sub = function() {
        function jQuerySub(selector, context) {
            return new jQuerySub.fn.init(selector, context);
        }
        jQuery.extend(true, jQuerySub, this);
        jQuerySub.superclass = this;
        jQuerySub.fn = jQuerySub.prototype = this();
        jQuerySub.fn.constructor = jQuerySub;
        jQuerySub.sub = this.sub;
        jQuerySub.fn.init = function init(selector, context) {
            if (context && context instanceof jQuery && !(context instanceof jQuerySub)) {
                context = jQuerySub(context);
            }

            return jQuery.fn.init.call(this, selector, context, rootjQuerySub);
        };
        jQuerySub.fn.init.prototype = jQuerySub.fn;
        var rootjQuerySub = jQuerySub(document);
        return jQuerySub;
    };

})();

jQuery(document).ready(function() {
    "use strict";
    jQuery(".responsive-menu").click(function(e) {
        jQuery(".main-nav>ul").css({
            display: "block"
        });
        e.stopPropagation();
        if (e.preventDefault)
            e.preventDefault();
        return false;
    });
    jQuery("body").click(function() {
        jQuery(".main-nav>ul").css({
            display: "none"
        });
    });
});


/* ================= IE fix ================= */
$(document).ready(function() {
    "use strict";
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function(obj, start) {
            for (var i = (start || 0), j = this.length; i < j; i++) {
                if (this[i] === obj) {
                    return i;
                }
            }
            return -1;
        };
    }
});

/* ================= END PLACE HOLDER ================= */

jQuery('.contact-form').each(function() {
    "use strict";
    var t = jQuery(this);
    var t_result = jQuery(this).find('.form-send');
    var t_result_init_val = t_result.val();
    var validate_email = function validateEmail(email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };
    var t_timeout;
    t.submit(function(event) {
        event.preventDefault();
        var t_values = {};
        var t_values_items = t.find('input[name],textarea[name]');
        t_values_items.each(function() {
            t_values[this.name] = jQuery(this).val();
        });
        if (t_values['contact-name'] === '' || t_values['contact-email'] === '' || t_values['contact-message'] === '') {
            t_result.val('Please fill in all the required fields.');
        } else
        if (!validate_email(t_values['contact-email']))
            t_result.val('Please provide a valid e-mail.');
        else
            jQuery.post("php/contacts.php", t.serialize(), function(result) {
                t_result.val(result);
            });
        clearTimeout(t_timeout);
        t_timeout = setTimeout(function() {
            t_result.val(t_result_init_val);
        }, 3000);
    });

});






// Navigation (A.B.)
jQuery(function($) {
    function load_navigation() {
        var menu_links = $('.main-nav ul li a').filter(function() {
            var s = $(this).data('anchor') ? '#' + $(this).data('anchor') : $(this).attr('href');
            if ($(s).length)
                return true;
            else
                return false;
        }).sort(function(a, b) {
            var as = $(a).data('anchor') ? '#' + $(a).data('anchor') : $(a).attr('href');
            var bs = $(b).data('anchor') ? '#' + $(b).data('anchor') : $(b).attr('href');
            return $(as).offset().top - $(bs).offset().top;
        });
        var menu_links_parents = menu_links.parent();
        var scrollSpyNavigation_flag = true;
        var scrollSpyNavigation_loop_flag = false;
        var scrollSpyNavigation_loop_time = 100;
        $('.main-nav ul li a').not(menu_links).parent().addClass('no-anchor');

        function scrollSpyNavigation() {
            if (scrollSpyNavigation_flag) {
                scrollSpyNavigation_flag = false;
                scrollSpyNavigation_action();
                setTimeout(scrollSpyNavigation_loop, scrollSpyNavigation_loop_time);
            } else {
                scrollSpyNavigation_loop_flag = true;
            }
        }

        function scrollSpyNavigation_loop() {
            if (scrollSpyNavigation_loop_flag) {
                scrollSpyNavigation_loop_flag = false;
                scrollSpyNavigation_action();
                setTimeout(scrollSpyNavigation_loop, scrollSpyNavigation_loop_time);
            } else {
                scrollSpyNavigation_flag = true;
            }
        }

        function scrollSpyNavigation_action() {

            if (!menu_links.length) return;

            var delta = 20;

            var targetOffset = $(window).scrollTop() + $('.navbar').height() + $('#wpadminbar').height() + delta;
            var i = -1;
            var i_parent;
            var i_buffer;

            while (i + 1 < menu_links.length && targetOffset >= $(menu_links.eq(i + 1).data('anchor') ? '#' + menu_links.eq(i + 1).data('anchor') : menu_links.eq(i + 1).attr('href')).offset().top) i++;

            i_buffer = i;
            while (i_buffer > 0 && ($(menu_links.eq(i).data('anchor') ? '#' + menu_links.eq(i).data('anchor') : menu_links.eq(i).attr('href')).offset().top) === ($(menu_links.eq(i_buffer - 1).data('anchor') ? '#' + menu_links.eq(i_buffer - 1).data('anchor') : menu_links.eq(i_buffer - 1).attr('href')).offset().top)) i_buffer--;

            menu_links_parents.filter('.active').each(function(index, element) {

                var t = $(element);
                var t_link = t.children('a');
                var t_link_index = menu_links.index(t_link);

                if (t_link_index < i_buffer || t_link_index > i)
                    t.removeClass('active');

            });

            while (i_buffer <= i) {

                menu_links.eq(i_buffer).parent().addClass('active');
                if (1 <= i_buffer)
                    $('.header').addClass('header-transform');
                else
                    $('.header').removeClass('header-transform');
                i_buffer++;

            }
        }

        function scrollToElement(target, duration) {
            if (!target.length) return;
            $('body,html').animate({
                scrollTop: target.offset().top
            }, 1000, 'swing');
        }
        menu_links.bind('click', function(e) {
            var s = $(this).data('anchor') ? '#' + $(this).data('anchor') : $(this).attr('href');
            e.preventDefault();
            scrollToElement($(s));
        });
        $(document).ready(function() {
            scrollSpyNavigation();
        });
        $(window).load(function() {
            scrollSpyNavigation();
        });
        $(window).scroll(function() {
            scrollSpyNavigation();
        });
    }
    load_navigation();
});

//==============END TWITTER====================================