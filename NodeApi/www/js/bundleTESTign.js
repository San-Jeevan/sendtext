// Sticky Plugin v1.0.0 for jQuery
// =============
// Author: Anthony Garand
// Improvements by German M. Bravo (Kronuz) and Ruud Kamphuis (ruudk)
// Improvements by Leonardo C. Daronco (daronco)
// Created: 2/14/2011
// Date: 2/12/2012
// Website: http://labs.anthonygarand.com/sticky
// Description: Makes an element on the page stick on the screen as you scroll
//       It will only set the 'top' and 'position' of your element, you
//       might need to adjust the width in some cases.

(function($) {
  var defaults = {
      topSpacing: 0,
      bottomSpacing: 0,
      className: 'is-sticky',
      wrapperClassName: 'sticky-wrapper',
      center: false,
      getWidthFrom: ''
    },
    $window = $(window),
    $document = $(document),
    sticked = [],
    windowHeight = $window.height(),
    scroller = function() {
      var scrollTop = $window.scrollTop(),
        documentHeight = $document.height(),
        dwh = documentHeight - windowHeight,
        extra = (scrollTop > dwh) ? dwh - scrollTop : 0;

      for (var i = 0; i < sticked.length; i++) {
        var s = sticked[i],
          elementTop = s.stickyWrapper.offset().top,
          etse = elementTop - s.topSpacing - extra;

        if (scrollTop <= etse) {
          if (s.currentTop !== null) {
            s.stickyElement
              .css('position', '')
              .css('top', '');
            s.stickyElement.parent().removeClass(s.className);
            s.currentTop = null;
          }
        }
        else {
          var newTop = documentHeight - s.stickyElement.outerHeight()
            - s.topSpacing - s.bottomSpacing - scrollTop - extra;
          if (newTop < 0) {
            newTop = newTop + s.topSpacing;
          } else {
            newTop = s.topSpacing;
          }
          if (s.currentTop != newTop) {
            s.stickyElement
              .css('position', 'fixed')
              .css('top', newTop);

            if (typeof s.getWidthFrom !== 'undefined') {
              s.stickyElement.css('width', $(s.getWidthFrom).width());
            }

            s.stickyElement.parent().addClass(s.className);
            s.currentTop = newTop;
          }
        }
      }
    },
    resizer = function() {
      windowHeight = $window.height();
    },
    methods = {
      init: function(options) {
        var o = $.extend(defaults, options);
        return this.each(function() {
          var stickyElement = $(this);

          var stickyId = stickyElement.attr('id');
          var wrapper = $('<div></div>')
            .attr('id', stickyId + '-sticky-wrapper')
            .addClass(o.wrapperClassName);
          stickyElement.wrapAll(wrapper);

          if (o.center) {
            stickyElement.parent().css({width:stickyElement.outerWidth(),marginLeft:"auto",marginRight:"auto"});
          }

          if (stickyElement.css("float") == "right") {
            stickyElement.css({"float":"none"}).parent().css({"float":"right"});
          }

          var stickyWrapper = stickyElement.parent();
          stickyWrapper.css('height', stickyElement.outerHeight());
          sticked.push({
            topSpacing: o.topSpacing,
            bottomSpacing: o.bottomSpacing,
            stickyElement: stickyElement,
            currentTop: null,
            stickyWrapper: stickyWrapper,
            className: o.className,
            getWidthFrom: o.getWidthFrom
          });
        });
      },
      update: scroller
    };

  // should be more efficient than using $window.scroll(scroller) and $window.resize(resizer):
  if (window.addEventListener) {
    window.addEventListener('scroll', scroller, false);
    window.addEventListener('resize', resizer, false);
  } else if (window.attachEvent) {
    window.attachEvent('onscroll', scroller);
    window.attachEvent('onresize', resizer);
  }

  $.fn.sticky = function(method) {
    if (methods[method]) {
      return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
    } else if (typeof method === 'object' || !method ) {
      return methods.init.apply( this, arguments );
    } else {
      $.error('Method ' + method + ' does not exist on jQuery.sticky');
    }
  };
  $(function() {
    setTimeout(scroller, 0);
  });
})(jQuery);

var api_endpoint = 'http://localhost:1234';
//var api_endpoint = '';
var options = {};
var inst;
var instrecv;
var clipboard = new Clipboard('#btnCopy');
var clipboardCopyCode = new Clipboard('#btnCopyCode');
var sendmodalopen = false;
var timerid = 0;

function rcvClicked() {
    event.preventDefault();

    var recvText = $("#inputReceive").val();
    recvText  = recvText.replace(/ /g, "");
    if (recvText == null || recvText === '') {
        alert("No Code entered!");
        $("#inputReceive").focus();
        return;
    }
    if (recvText.length !== 6) {
        alert("Code is always 6 digits!");
        $("#inputReceive").focus();
        return;
    }
    StartReceiveSession(recvText);
}


function sendClicked() {
    event.preventDefault();
    var sendText = $("#inputSend").val();
    if (sendText == null || sendText === '') {
        alert("Cannot send empty message!");
        $("#inputSend").focus();
        return;
    }
    StartSendSession({ data: sendText });
}


function initClipboardjs() {

    //Copy Received Text
    clipboard.on('success', function (e) {
        e.clearSelection();
        $("#btnCopy").text("Copied successfully!");
    });

    clipboard.on('error', function (e) {
        $("#btnCopy").text("Could not copy to clipboard");
        $("#btnCopy").css('background', 'red');
    });


    //Copy Code
    clipboardCopyCode.on('success', function (e) {
        e.clearSelection();
        $("#btnCopyCode").css('background', 'lightgreen');
    });

    clipboardCopyCode.on('error', function (e) {
        $("#btnCopyCode").css('background', 'red');
    });
}


function initBindings() {
    $("#btnReceive").click(rcvClicked);
    $("#btnSend").click(sendClicked);
    $('[data-toggle="popover"]').popover();
    inst = $('[data-remodal-id=modal]').remodal();
    instrecv = $('[data-remodal-id=modalrecv]').remodal();
    $(document).on('closed', '.remodal', function (e) {
        sendmodalopen = false;
        $("#btnCopyCode").css('background', '#1ab0db');
    });

}


jQuery(document).ready(function () {
    "use strict";
    initBindings();
    initClipboardjs();
});



function StartReceiveSession(code) {
    $.ajax({
        type: "GET",
        url: api_endpoint + "/api/getSession/" + code,
        crossDomain: true,
        success: function (data, status) {
            if (data.success) {
                $("#popuptextarea").val(data.returnobject);
                instrecv.open();
                $("#popuptextarea").select();
            }
        },
        error: function (jqXHR, status) {
            console.log(jqXHR);
        }
    });
}



function StartSendSession(data) {
    $.ajax({
        type: "POST",
        url: api_endpoint + "/api/createSession",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        crossDomain: true,
        dataType: "json",
        success: function (data, status) {
            if (data.success) {
                var code = data.returnobject.toString().substring(0, 3) + " " + data.returnobject.toString().substring(3, 6);
                $("#popupcodespan").val(code);
                inst.open();

                var tenminutes = 60 * 10,
                    display = $('#popuptimeleft');
                sendmodalopen = true;
                startTimer(tenminutes, display);

            }
        },
        error: function (jqXHR, status) {
            console.log(jqXHR);
        }
    });
}





function startTimer(duration, display) {
    $('#progressbar').show();
    var timer = duration, minutes, seconds;
    timerid = setInterval(function () {
        if (!sendmodalopen) clearInterval(timerid);
        minutes = parseInt(timer / 60, 10);
        seconds = parseInt(timer % 60, 10);

        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;

        display.text(minutes + ":" + seconds + " remain before expiry");

        var percent = (timer / duration) * 100;
        $('#progressbar').attr('aria-valuenow', percent).css('width', percent + "%");
        if (--timer < 0) {
            $('#progressbar').hide();
            $("#popupcodespan").val("EXPIRED");
            clearInterval(timerid);
        }
    }, 1000);
}


(function ( $ ) {

	"use strict";

	$.tesla_responsive = function( options ) {

		var $window;

		var length;

		var index;

		var previous;

		var resize;

		if ($.isArray(options)) {

		    $window = $(window);

		    length = options.length;

		    if(length){

		    	options.sort(function(a, b){

		    		return b.width - a.width;

		    	});

		    	resize = function () {

		    	    var width = $window.width();

		    	    var i = 0;

		    	    while(i < length && width < options[i].width){

		    	    	i++;

		    	    }

		    	    index = i < length ? i : undefined;

		    	    if (previous !== index) {

		    	        previous = index;

		    	        if(undefined !== index){

		    	        	options[index].action();

		    	        }

		    	    }

		    	};

		    	$window.resize(resize);

		    	resize();

		    }

		}

		return resize;

	};
	
	$.fn.tesla_slider = function( options ) {

		return this.each(function(i, e){

			var $e = $(e);

			var settings = $.extend({

				item: '.item',
				next: '.next',
				prev: '.prev',
				container: $e,
				autoplay: true,
				autoplaySpeed: 4000,
				autoplayResume: 4000,
				bullets: '[data-tesla-plugin="bullets"]',
				//active: 'active' //class for current slide - to be implemented

			},options,{

				item: $e.attr('data-tesla-item'),
				next: $e.attr('data-tesla-next'),
				prev: $e.attr('data-tesla-prev'),
				container: $e.attr('data-tesla-container'),
				autoplay: $e.attr('data-tesla-autoplay')!=="false",
				autoplaySpeed: $e.attr('data-tesla-autoplay-speed') ? parseInt($e.attr('data-tesla-autoplay-speed'), 10) : $e.attr('data-tesla-autoplay-speed'),
				autoplayResume: $e.attr('data-tesla-autoplay-resume') ? parseInt($e.attr('data-tesla-autoplay-resume'), 10) : this.autoplaySpeed,
				bullets: $e.attr('data-tesla-bullets'),
				active: $e.attr('data-tesla-active')

			});

			var container = settings.container instanceof jQuery ? settings.container : $e.find(settings.container);

			var items = container.find(settings.item);

			var bullets = $e.find(settings.bullets);

			var next = $e.find(settings.next);

			var prev = $e.find(settings.prev);

			var max = items.length - 1;

			var index = 0;

			var prev_action;

			var next_action;

			var goto_action;

			var process;

			var process_first;

			var autoplay_interval;

			var autoplay_timeout;

			var autoplay_play;

			var autoplay_stop;

			var autoplay_resume;

			var imagesLoaded_object;

			var imagesLoaded_progress;

			if(max <= 0) return;

			tesla_set_option($e, 'slider', 'tesla_remove', 'removed', false);

			prev_action = function(){

				var index_old = index;
				var index_new;

				index--;

				if( index < 0 )
					index = max;

				index_new = index;

				container.css({
					height: Math.max(items.eq(index_old).outerHeight(true), items.eq(index_new).outerHeight(true))
				});

				items.eq(index_old).stop(true, true).fadeOut(1000, function(){
					
				});
				items.eq(index).stop(true, true).fadeIn(1000, function(){
					container.css({
						height: items.eq(index_new).outerHeight(true)
					});
				});

				bullets.trigger('teslaSliderChange', {'index': index});

			};

			next_action = function(){

				var index_old = index;
				var index_new;

				index++;

				if( index > max )
					index = 0;

				index_new = index;

				container.css({
					height: Math.max(items.eq(index_old).outerHeight(true), items.eq(index_new).outerHeight(true))
				});

				items.eq(index_old).stop(true, true).fadeOut(1000, function(){

				});
				items.eq(index).stop(true, true).fadeIn(1000, function(){
					container.css({
						height: items.eq(index_new).outerHeight(true)
					});
				});

				bullets.trigger('teslaSliderChange', {'index': index});

			};

			goto_action = function(goto_index){

				if(goto_index === index) return;

				var index_old = index;
				var index_new;

				index = goto_index;

				if( index > max )
					index = 0;

				if( index < 0 )
					index = max;

				index_new = index;

				container.css({
					height: Math.max(items.eq(index_old).outerHeight(true), items.eq(index_new).outerHeight(true))
				});

				items.eq(index_old).stop(true, true).fadeOut(1000, function(){

				});
				items.eq(index).stop(true, true).fadeIn(1000, function(){
					container.css({
						height: items.eq(index_new).outerHeight(true)
					});
				});

				bullets.trigger('teslaSliderChange', {'index': index});

			};

			process_first = function(){

				container.css({
					position: 'relative',
					height: items.eq(index).outerHeight(true)
				});
				items.css({
					position: 'absolute',
					top: 0,
					left: 0,
					right: 0
				});
				items.filter(':gt(0)').css({
					display: 'none'
				});

				$(window).resize(function(){

					container.css({
						height: items.eq(index).outerHeight(true)
					});

				});

			}

			process = function(){

				process_first();

				prev.click(function(ev){

					autoplay_stop();
					prev_action();
					autoplay_resume();

					if(ev.preventDefault)
						ev.preventDefault();
					else
						return false;

				});
				tesla_set_option($e, 'slider', 'prev', 'event', prev);

				next.click(function(ev){

					autoplay_stop();
					next_action();
					autoplay_resume();

					if(ev.preventDefault)
						ev.preventDefault();
					else
						return false;

				});
				tesla_set_option($e, 'slider', 'next', 'event', next);

				bullets.on('teslaBulletsClick', function(ev, data){

					autoplay_stop();
					goto_action(data.index);
					autoplay_resume();

					if(ev.preventDefault)
						ev.preventDefault();
					else
						return false;

				});
				tesla_set_option($e, 'slider', 'bullets', 'bullets', bullets);

				items.hover(function(){

					autoplay_stop();

				},function(){

					autoplay_stop();
					autoplay_resume();

				});

				autoplay_play = function(){

					if(!settings.autoplay) return;

					autoplay_interval = setInterval(next_action, settings.autoplaySpeed);

					tesla_set_option($e, 'slider', 'autoplay_interval', 'interval', autoplay_interval);

				};

				autoplay_stop = function(){

					if(!settings.autoplay) return;

					clearInterval(autoplay_interval);
					clearTimeout(autoplay_timeout);

				};

				autoplay_resume = function(){

					if(!settings.autoplay) return;

					autoplay_timeout = setTimeout(autoplay_play, settings.autoplayResume);

					tesla_set_option($e, 'slider', 'autoplay_timeout', 'timeout', autoplay_timeout);

				};

				autoplay_play();

			};

			// process_first();

			if(imagesLoaded){

				imagesLoaded(container[0], function(){

					if(!tesla_get_option($e, 'slider', 'tesla_remove').value)
						process();

				});

			}else{

				process();

			}

		});

	};

	$.fn.tesla_masonry = function( options ) {

		return this.each(function(i, e){

			var $e = $(e);

			var settings = $.extend({

				filters: '[data-tesla-plugin="filters"]'

			},options,{

				filters: $e.attr('data-tesla-filters')

			});

			var filters = $(settings.filters);

			var items = $e.children();

			var process;

			tesla_set_option($e, 'masonry', 'tesla_remove', 'removed', false);

			process = function(){

				$e.masonry();

				if(filters.length){

					filters.on('teslaFiltersChange', function(ev, data){

						var i;

						var n = data.categories.length;

						var selector = '';

						var masonry_object = $e.data('masonry');

						for(i=0; i<n; i++){

							if(i)
								selector += ', ';

							selector += '.' + data.categories[i];

						}

						if(''===selector){

							masonry_object.options.itemSelector = undefined;

							items.stop(true, true).fadeIn(400);

						}else{

							masonry_object.options.itemSelector = selector;

							items.stop(true, true);

							items.filter(selector).fadeIn(400);

							items.not(selector).fadeOut(400);

						}

						masonry_object.reloadItems();

						masonry_object.layout();

					});
					tesla_set_option($e, 'masonry', 'filters', 'filters', filters);

				}

			};

			if($.fn.masonry){

				if(imagesLoaded){

					imagesLoaded(e, function(){

						if(!tesla_get_option($e, 'masonry', 'tesla_remove').value)
							process();

					});

				}else{

					process();

				}

			}

		});

	};

	$.fn.tesla_filters = function( options ) {

		return this.each(function(i, e){

			var $e = $(e);

			var settings = $.extend({

				categories: '[data-category]'

			},options,{

				categories: $e.attr('data-tesla-category')

			});

			var categories = $e.find(settings.categories);

			categories.click(function(ev){

				var t = $(this);

				var cat_array;

				if(t.hasClass('active')){

					if(''===t.attr('data-category')){

						categories.filter('[data-category=""]').removeClass('active');

						categories.filter('[data-category!=""]').addClass('active');

					}else{

						categories.filter(t).removeClass('active');

						if(!categories.filter('.active').length)
							categories.filter('[data-category=""]').addClass('active');
						
					}

				}else{

					if(''===t.attr('data-category')){

						categories.filter('[data-category=""]').addClass('active');

						categories.filter('[data-category!=""]').removeClass('active');

					}else{

						categories.filter('[data-category=""]').removeClass('active');

						categories.filter(t).addClass('active');
						
					}

				}

				cat_array = categories.filter('.active[data-category!=""]').map(function(){

					return $(this).attr('data-category');

				}).get();

				$e.trigger('teslaFiltersChange', {'categories': cat_array});

				if(ev.preventDefault)
					ev.preventDefault();
				else
					return false;

			});
			tesla_set_option($e, 'filters', 'categories', 'event', categories);

		});

	};

	$.fn.tesla_bullets = function( options ) {

		return this.each(function(i, e){

			var $e = $(e);

			var settings = $.extend({

				bullets: '>*',
				hover: false

			},options,{

				bullets: $e.attr('data-tesla-selector'),
				hover: $e.attr('data-tesla-hover')

			});

			var bullets = $e.find(settings.bullets);

			var hover = '0' === settings.hover || ( 'string' === typeof(settings.hover) && 'false' === settings.hover.toLowerCase() ) ? false : Boolean(settings.hover);

			var action = function(ev){

				$e.trigger('teslaBulletsClick', {'index': bullets.index(this)});

			};

			bullets.eq(0).addClass('active');

			bullets.click(action);
			tesla_set_option($e, 'bullets', 'bullets', 'event', bullets);

			if(hover) bullets.mouseover(action);

			$e.on('teslaSliderChange', function(ev, data){

				bullets.filter('.active').removeClass('active');

				bullets.eq(data.index).addClass('active');

			});
			tesla_set_option($e, 'bullets', 'e', 'event', $e);

		});

	};

	$.fn.tesla_carousel = function( options ) {

		return this.each(function(i, e){

			var $e = $(e);

			var settings = $.extend({

				item: '.item',
				next: '.next',
				prev: '.prev',
				container: $e,
				rotate: true,
				autoplay: true,
				hideEffect: true,
				autoplaySpeed: 4000,
				autoplayResume: 4000

			},options,{

				item: $e.attr('data-tesla-item'),
				next: $e.attr('data-tesla-next'),
				prev: $e.attr('data-tesla-prev'),
				container: $e.attr('data-tesla-container'),
				rotate: $e.attr('data-tesla-rotate'),
				autoplay: $e.attr('data-tesla-autoplay'),
				hideEffect: $e.attr('data-tesla-hide-effect'),
				autoplaySpeed: $e.attr('data-tesla-autoplay-speed') ? parseInt($e.attr('data-tesla-autoplay-speed'), 10) : $e.attr('data-tesla-autoplay-speed'),
				autoplayResume: $e.attr('data-tesla-autoplay-resume') ? parseInt($e.attr('data-tesla-autoplay-resume'), 10) : this.autoplaySpeed,

			});

			var container = settings.container instanceof jQuery ? settings.container : $e.find(settings.container);

			var items = container.find(settings.item);

			var next = $e.find(settings.next);

			var prev = $e.find(settings.prev);

			var max;

			var visible;

			var index = 0;

			var prev_action, prev_action_move;

			var next_action, next_action_move;

			var action_0, action_768, action_992, action_1200, action_responsive, action_adjust;

			var process;

			var responsive;

			var item_width;

			var item_height;

			var rotate = '0' === settings.rotate || ( 'string' === typeof(settings.rotate) && 'false' === settings.rotate.toLowerCase() ) ? false : Boolean(settings.rotate);

			var rotate_interval;

			var autoplay = '0' === settings.autoplay || ( 'string' === typeof(settings.autoplay) && 'false' === settings.autoplay.toLowerCase() ) ? false : Boolean(settings.autoplay);

			var hide_effect = '0' === settings.hideEffect || ( 'string' === typeof(settings.hideEffect) && 'false' === settings.hideEffect.toLowerCase() ) ? false : Boolean(settings.hideEffect);

			var autoplay_interval, autoplay_timeout;

			var autoplay_start, autoplay_stop, autoplay_resume;

			var responsive_resize;

			tesla_set_option($e, 'carousel', 'tesla_remove', 'removed', false);

			container = container.wrapInner('<div>').children();

			tesla_set_option($e, 'carousel', 'container', 'wrapper', container);

			autoplay_start = function(){

				if(!autoplay) return;

				autoplay_stop();

				if(undefined !== rotate_interval)
					autoplay_interval = 0;
				else{
					autoplay_interval = setInterval(next_action, settings.autoplaySpeed);
					tesla_set_option($e, 'carousel', 'autoplay_interval', 'interval', autoplay_interval);
				}

			};

			autoplay_stop = function(){

				if(!autoplay) return;

				clearInterval(autoplay_interval);
				autoplay_interval = undefined;

				clearTimeout(autoplay_timeout);
				autoplay_timeout = undefined;

			};

			autoplay_resume = function(){

				if(!autoplay) return;

				autoplay_stop();

				if(undefined !== rotate_interval)
					autoplay_timeout = 0;
				else{
					autoplay_timeout = setTimeout(autoplay_start, settings.autoplayResume);
					tesla_set_option($e, 'carousel', 'autoplay_timeout', 'timeout', autoplay_timeout);
				}

			};

			prev_action_move = function(){

				if(index > 0){

					if(hide_effect)
						items.eq(index + visible - 1).css({

							'-webkit-transform': 'scale(0)',
							'-moz-transform': 'scale(0)',
							'-ms-transform': 'scale(0)',
							'-o-transform': 'scale(0)',
							'transform': 'scale(0)'

						});

					if(!autoplay && index === max){

						next.removeClass('disabled');

					}

					index--;

					if(!autoplay && index === 0){

						prev.addClass('disabled');

					}

					if(hide_effect)
						items.eq(index).css({

							'-webkit-transform': 'scale(1)',
							'-moz-transform': 'scale(1)',
							'-ms-transform': 'scale(1)',
							'-o-transform': 'scale(1)',
							'transform': 'scale(1)'

						});

					container.css({ left: String( - index * 100 / visible ) + '%'  });

					return true;

				}else return false;

			};

			prev_action = function(){

				if(undefined !== rotate_interval) return;

				if(prev_action_move()){

					// good

				}else{

					if(rotate && index < max){

						clearInterval(rotate_interval);

						rotate_interval = setInterval(function(){

							if(!next_action_move()){

								clearInterval(rotate_interval);
								rotate_interval = undefined;

							}

						}, 200);
						tesla_set_option($e, 'carousel', 'rotate_interval', 'interval', rotate_interval);

					}

				}

			};

			next_action_move = function(){

				if(index < max){

					if(hide_effect)
						items.eq(index).css({

							'-webkit-transform': 'scale(0)',
							'-moz-transform': 'scale(0)',
							'-ms-transform': 'scale(0)',
							'-o-transform': 'scale(0)',
							'transform': 'scale(0)'

						});

					if(!autoplay && index === 0){

						prev.removeClass('disabled');

					}

					index++;

					if(!autoplay && index === max){

						next.addClass('disabled');

					}

					if(hide_effect)
						items.eq(index + visible - 1).css({

							'-webkit-transform': 'scale(1)',
							'-moz-transform': 'scale(1)',
							'-ms-transform': 'scale(1)',
							'-o-transform': 'scale(1)',
							'transform': 'scale(1)'

						});

					container.css({ left: String( - index * 100 / visible ) + '%'  });

					return true;

				}else return false;

			};

			next_action = function(){

				if(undefined !== rotate_interval) return;

				if(next_action_move()){

					// good

				}else{

					if(rotate && index > 0){

						clearInterval(rotate_interval);

						clearInterval(autoplay_interval);
						clearTimeout(autoplay_timeout);

						rotate_interval = setInterval(function(){

							if(!prev_action_move()){

								clearInterval(rotate_interval);
								rotate_interval = undefined;

								if(undefined !== autoplay_interval) autoplay_start();
								if(undefined !== autoplay_timeout) autoplay_resume();

							}

						}, 200);
						tesla_set_option($e, 'carousel', 'rotate_interval', 'interval', rotate_interval);

					}

				}

			};

			action_0 = function(){

				// console.log('0-767');

			    action_responsive();

			};

			action_768 = function(){

				// console.log('768-991');

			    action_responsive();

			};

			action_992 = function(){

				// console.log('992-1199');

			    action_responsive();

			};

			action_1200 = function(){

				// console.log('1200+');

			    action_responsive();

			};

			action_responsive = function(){

				item_height = Math.max.apply(null, items.map(function(){

					return $(this).outerHeight(true);

				}).get());

			    item_width = items.outerWidth(true);

			    visible = Math.round( container.width() / item_width );

			    max = Math.max(items.length - visible, 0);

			    index = Math.min(index, max);

			    if(!max){

			    	prev.addClass('disabled');
			    	next.addClass('disabled');

			    }else{

			    	if(!autoplay && index === 0)
			    		prev.addClass('disabled');
			    	else
			    		prev.removeClass('disabled');

			    	if(!autoplay && index === max)
			    		next.addClass('disabled');
			    	else
			    		next.removeClass('disabled');

			    }

				container.css({

					position: 'relative',
					height: item_height,
					'-webkit-transition': 'left 0.4s ease-out',
					'-moz-transition': 'left 0.4s ease-out',
					'-ms-transition': 'left 0.4s ease-out',
					'-o-transition': 'left 0.4s ease-out',
					'transition': 'left 0.4s ease-out',
					left: String( - index * 100 / visible ) + '%'

				});

				items.css({

					position: 'absolute',
					top: 0,
					'-webkit-transition': '-webkit-transform 0.4s ease-out',
					'-moz-transition': '-webkit-transform 0.4s ease-out',
					'-ms-transition': '-webkit-transform 0.4s ease-out',
					'-o-transition': '-webkit-transform 0.4s ease-out',
					'transition': '-webkit-transform 0.4s ease-out'

				}).each(function(i){

					$(this).css({ left: String( i * 100 / visible ) + '%'  });

				});

				action_adjust();

			};

			action_adjust = function(){

				if(hide_effect){

					items.filter(':gt('+String(visible)+'),:eq('+String(visible)+'),:lt('+String(index)+')').css({
						'-webkit-transform': 'scale(0)',
						'-moz-transform': 'scale(0)',
						'-ms-transform': 'scale(0)',
						'-o-transform': 'scale(0)',
						'transform': 'scale(0)'
					});

					items.filter(':gt('+String(index)+'):lt('+String(visible)+'),:eq('+String(index)+')').css({
						'-webkit-transform': 'scale(1)',
						'-moz-transform': 'scale(1)',
						'-ms-transform': 'scale(1)',
						'-o-transform': 'scale(1)',
						'transform': 'scale(1)'
					});

				}

			}

			responsive = [ { width: 0, action: action_0 }, { width: 768, action: action_768 }, { width: 992, action: action_992 }, { width: 1200, action: action_1200 } ];

			process = function(){

				responsive_resize = $.tesla_responsive(responsive);
				tesla_set_option($e, 'carousel', 'responsive_resize', 'responsive', responsive_resize);

				prev.click(function(ev){

					autoplay_stop();
					autoplay_resume();

					prev_action();

					if(ev.preventDefault)
						ev.preventDefault();
					else
						return false;

				});
				tesla_set_option($e, 'carousel', 'prev', 'event', prev);

				next.click(function(ev){

					autoplay_stop();
					autoplay_resume();

					next_action();

					if(ev.preventDefault)
						ev.preventDefault();
					else
						return false;

				});
				tesla_set_option($e, 'carousel', 'next', 'event', next);

				items.click(function(ev){

					autoplay_stop();
					autoplay_resume();

				});

				items.hover(function(ev){

					autoplay_stop();

				},function(ev){

					autoplay_stop();
					autoplay_resume();

				});
				tesla_set_option($e, 'carousel', 'items', 'event', items);

				autoplay_start();

			};

			if(imagesLoaded){

				imagesLoaded(container[0], function(){

					if(!tesla_get_option($e, 'carousel', 'tesla_remove').value)
						process();

				});

			}else{

				process();

			}

		});

	};

	$.fn.tesla_news_ticker = function( options ) {

		return this.each(function(i, e){

			var $e = $(e);

			var settings = $.extend({

				speed: 20,
				item: '.item',
				container: $e

			},options,{

				speed: $e.attr('data-tesla-speed'),
				item: $e.attr('data-tesla-item'),
				container: $e.attr('data-tesla-container')

			});

			var container = settings.container instanceof jQuery ? settings.container : $e.find(settings.container);

			var items = container.find(settings.item);

			var process;

			if(!items.length) return;

			process = function(){

				var items_width;

				var items_clone;

				var aloz;

				container.css({
					overflow: 'hidden'
				});

				container = items.wrapAll('<div>').parent().addClass('no-style');

				container.css({
					overflow: 'hidden'
				});

				items_clone = items.eq(0).clone()

				items.last().after(items_clone);

				items = items.add(items_clone);

				items = items.wrap('<div>').parent().addClass('no-style');

				items.css({
					float: 'left',
					overflow: 'hidden'
				});

				items_width = 0;

				items.each(function(i, e){
					items_width += $(e).width();
				});

				items_width += items.length;

				container.css({
					width: items_width
				});

				aloz = function(){

					var item_current;
					var item_current_width;

					var wei = function(){

						item_current = items.parent().children().first();
						item_current_width = item_current.width();

					};

					var dara = function(){

						var item_current_margin = parseInt(item_current.css('margin-left'), 10) || 0;
						var item_current_distance = item_current_width+item_current_margin;

						item_current.animate({
							marginLeft: -item_current_width
						},{
							duration: item_current_distance*settings.speed,
							easing: 'linear',
							queue: false,
							start: function(){
								tesla_set_option($e, 'news_ticker', 'item_current', 'animation', item_current);
							},
							done: function(){
								item_current.css({
									display: 'none',
									marginLeft: 0
								}).appendTo(item_current.parent()).css({
									display: 'block'
								});
								wei();
								dara();
							}
						});

					};

					wei();
					dara();

					items.hover(function(){
						item_current.stop();
					},function(){
						dara();
					});

				};

				setTimeout(aloz, 1000);

			};

			process();

		});

	};
	
	$.fn.tesla_remove = function() {

		return this.each(function(i, e){

			var $e = $(e);

			var options = tesla_get_options($e);

			var plugin, key;

			if(undefined!==options){

				for(plugin in options){

					if(undefined!==options[plugin]){

						tesla_set_option($e, plugin, 'tesla_remove', 'removed', true);

						for(key in options[plugin]){

							switch(options[plugin][key].type){

								case 'interval':
									clearInterval(options[plugin][key].value);
									break;

								case 'timeout':
									clearTimeout(options[plugin][key].value);
									break;

								case 'event':
									$(options[plugin][key].value).unbind().off();
									break;

								case 'bullets':
									$(options[plugin][key].value).unbind().off().tesla_remove();
									break;

								case 'filters':
									$(options[plugin][key].value).unbind().off().tesla_remove();
									break;

								case 'responsive':
									$(window).unbind('resize', options[plugin][key].value);
									break;

								case 'wrapper':
									$(options[plugin][key].value).contents().unwrap();
									break;

								case 'animation':
									$(options[plugin][key].value).stop();
									break;

								default:

							}

						}

					}

				}

			}

		});

	};

	$(function(){

		$('[data-tesla-plugin="slider"]').tesla_slider();

		$('[data-tesla-plugin="carousel"]').tesla_carousel();

		$('[data-tesla-plugin="masonry"]').tesla_masonry();

		$('[data-tesla-plugin="filters"]').tesla_filters();

		$('[data-tesla-plugin="bullets"]').tesla_bullets();

		$('[data-tesla-plugin="news_ticker"]').tesla_news_ticker();

	});

	function tesla_get_options($e){

		return $($e).data('tesla_themes');

	}

	function tesla_get_option($e, $plugin, $key){

		var $data = $($e).data('tesla_themes');

		var $result = undefined;

		if(undefined!==$data && undefined!==$data[$plugin] && undefined!==$data[$plugin][$key]){

			$result = $data[$plugin][$key];

		}

		return $result;

	}

	function tesla_set_option($e, $plugin, $key, $type, $value){

		var $data;
		$e = $($e);
		$data = $e.data('tesla_themes');

		if(undefined===$data){

			$data = {};

			$data[$plugin] = {};

			$data[$plugin][$key] = {'type': $type, 'value': $value};

		}else{

			if(undefined===$data[$plugin]){

				$data[$plugin] = {};

				$data[$plugin][$key] = {'type': $type, 'value': $value};

			}else{

				$data[$plugin][$key] = {'type': $type, 'value': $value};

			}

		}

		$e.data('tesla_themes', $data);
		
	}

}( jQuery ));
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