// Limit scope pollution from any deprecated API
jQuery(function($){

    var load_changer = function(){
        var t_body = $('body');
        var t_div = $('.box-wide');
        var t_transition_time = 0;
        var t_transition_apply = function(){
            clearTimeout(t_transition_time);
            t_body.addClass('animated_change');
            t_transition_time = setTimeout(t_transition_end, 1500);
        };
        var t_transition_end = function(){
            t_body.removeClass('animated_change');
        };
        var t_color = $('select[name="site_color"]');
        var t_layout = $('select[name="site_layout"]');
        t_color.change(function(){
            var t = $(this);
            if(t.val()!==t.data('color')){
                t_transition_apply();
                switch(t.val()){
                    case 'dark':
                        t_div.addClass('black_version');
                        break;
                    case 'light':
                        t_div.removeClass('black_version');
                        break;
                    default:
                        break;
                }
                t.data('color', t.val());
            }
        });
        t_layout.change(function(){
            var t = $(this);
            if(t.val()!==t.data('layout')){
                t_transition_apply();
                switch(t.val()){
                    case 'boxed':
                        t_body.addClass('boxed');
                        break;
                    case 'wide':
                        t_body.removeClass('boxed');
                        break;
                    default:
                        break;
                }
                t.data('layout', t.val());
            }
        });
        $('#background_patterns>li').click(function(){
            if('boxed'!==t_layout.data('layout')){
                t_layout.val('boxed');
                t_transition_apply();
                t_body.addClass('boxed');
                t_layout.data('layout','boxed');
            }
            t_body.css({backgroundImage: 'url('+$(this).children('img').attr('src')+')', backgroundRepeat: 'repeat'});
        });
        $('#background_images>li').click(function(){
            if('boxed'!==t_layout.data('layout')){
                t_layout.val('boxed');
                t_transition_apply();
                t_body.addClass('boxed');
                t_layout.data('layout','boxed');
            }
            t_body.css({backgroundImage: 'url('+$(this).children('img').attr('src')+')', backgroundRepeat: 'no-repeat'});
        });

        var t_box = $('.settings-box');
        var t_box_width = t_box.outerWidth();
        $('.settings-box-icon').click(function(){
            if(t_box.data('visible')){
                t_box.css({left: -t_box_width});
                t_box.data('visible', false);
            }else{
                t_box.css({left: 0});
                t_box.data('visible', true);
            }
        });

        var style_index = 0;

        function color_options(selector, default_color, color_change){
            var t_color = default_color;
            var t_picker = undefined;
            var t_picker_container = $('<div/>').addClass('color-selecter').css({position: 'fixed', zIndex: 999, top: 388, left: 200});
            var t_color_input = $('<input/>').css({position: 'absolute', zIndex: 999, top: 198, left: 0, width: '100%', textAlign: 'center', lineHeight: '1.6em', border: '1px solid black'});
            var style_id = 'settings-box-style'+style_index;
            style_index++;
            $('.settings-box-icon').click(function(){
                if(t_box.data('visible')){
                    t_picker_container.hide();
                }
            });
            var t_callback = function(){
                var t_style = $('head>#'+style_id);
                var t_output = '';
                if(!t_style.length)
                    t_style = $('<style/>').attr('id',style_id).appendTo('head');
                t_output += color_change.replace(/%color%/g,t_color);
                t_style.html(t_output);
            };
            $(selector+'>li:lt(-1)').click(function(){
                $('.color-selecter').hide();
                t_color = $(this).children('span').css('background-color').replace(/rgb\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/ig,function(c, r, g, b){return '#'+Number(r).toString(16)+Number(g).toString(16)+Number(b).toString(16);});
                t_callback();
                if(undefined!==t_picker){
                    t_picker.setColor(t_color);
                    t_color_input.val(t_color);
                }
            });
            var t_input_update = function(){
                t_color_input.val(t_color);
                t_color_input.css({backgroundColor: t_color, color: t_picker.hsl[2] > 0.5 ? '#000' : '#fff'});
            };
            $(selector+'>li:last').click(function(){
                if(undefined===t_picker){
                    t_picker = $.farbtastic(t_picker_container.appendTo('body'));
                    t_picker_container.append(t_color_input);
                    t_picker.setColor(t_color);
                    t_input_update();
                    t_picker.linkTo(function(color){
                        t_color = color;
                        t_callback();
                        t_input_update();
                    });
                    t_color_input.change(function(){
                        t_picker.setColor(t_color_input.val());
                    });
                    t_picker_container.show();
                }else{
                    $('.color-selecter').not(t_picker_container).hide();
                    t_picker_container.toggle();
                }
            });
        }

        color_options('#site_color','#e74c3c',"\n\
        .statistics-v1 .statistic-box h4,\n\
        .blog-v1 .blog-post-box .post-footer a i,\n\
        .working-progress-v1 .progress-box:hover .progress-hover h3,\n\
        .statistics-v1 .statistic-box .statistic-box-counter,\n\
        .blog-widget ul li h4 a:hover,\n\
        .twitter-widget ul li a,\n\
        .blog-v1 .blog-post-box .post-footer a:hover,\n\
        .main-nav ul li a:hover,\n\
        .responsive-menu:hover,\n\
        .text-widget a:hover,\n\
        .settings-box i,\n\
        .sidebar .widget .mini-post h3 a:hover,\n\
        .comments-area .comment .comment-info,\n\
        .main-nav ul li ul li a:hover,\n\
        .comments-area .comment .comment-info a,\n\
        .sidebar .widget-categories ul li a:hover,\n\
        .blog-post .post-footer .post-author h6,\n\
        .pricing-tables-v2 .pricing-box.pricing-box-featured .pricing-header h3,\n\
        .social.share-it li a,\n\
        .single-project .project-details li a,\n\
        .pricing-tables-v2 .pricing-box.pricing-box-featured .pricing-options li span,\n\
        .pricing-tables-v2 .pricing-box.pricing-box-featured .pricing-footer .pricing-link,\n\
        .main-nav ul li.active a {\n\
            color: %color%;\n\
        }\n\
        .features-v1:after,\n\
        .buy-bar-v1,\n\
        .team-v1,\n\
        .services-v1 .services-box:hover .services-content,\n\
        .pricing-tables-v1 .pricing-box.pricing-box-featured .pricing-footer .pricing-link,\n\
        .works-v1 .work-box .work-box-hover:after,\n\
        .pricing-tables-v1 .pricing-box.pricing-box-featured .pricing-price,\n\
        .pricing-tables-v1 .pricing-box.pricing-box-featured .pricing-header,\n\
        .pricing-tables-v1 .pricing-box .pricing-footer .pricing-link:hover,\n\
        .socials-bar-v1:after,\n\
        .button-2 {\n\
            background: %color%;\n\
        }\n\
        .comments-area .comments-line:focus,\n\
        .comments-area .comments-area:focus,\n\
        .sidebar .widget-search-form .search-line:focus,\n\
        .pricing-tables-v2 .pricing-box.pricing-box-featured .pricing-header,\n\
        .blog-v1 .blog-post-box .post-header .post-author img,\n\
        .pricing-tables-v1 .pricing-box.pricing-box-featured,\n\
        .working-progress-v1 .progress-box .progress-number,\n\
        .pricing-tables-v2 .pricing-box.pricing-box-featured .pricing-footer .pricing-link,\n\
        .pricing-tables-v2 .pricing-box.pricing-box-featured,\n\
        .single-project .slider .slider-dots li.active,\n\
        .button-2 {\n\
            border-color: %color%;\n\
        }\n\
        .collaborate-v1 .custom svg path,\n\
        .collaborate-v1 .custom svg polyline,\n\
        .collaborate-v1 .custom svg rect,\n\
        .collaborate-v1 .custom svg line,\n\
        .collaborate-v1 .custom svg circle,\n\
        .statistics-v2 .statistic-box h1 svg path,\n\
        .statistics-v2 .statistic-box h1 svg polyline,\n\
        .statistics-v2 .statistic-box h1 svg rect,\n\
        .statistics-v2 .statistic-box h1 svg line,\n\
        .statistics-v2 .statistic-box h1 svg circle {\n\
          stroke: %color% ;\n\
        }\n\
        .statistics-v2 {\n\
            border-top: 7px solid %color%;\n\
        }\n\
        .tab-widget .nav-tabs li.active a {\n\
            border-top: 4px solid %color%;\n\
        }\n\
        .services-v1 .services-box:hover .services-content:after {\n\
            border-color: transparent %color% transparent transparent;\n\
        }\n\
        ");
    };

    load_changer();

});