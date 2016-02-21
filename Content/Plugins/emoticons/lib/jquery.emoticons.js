(function (root, factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        define([
            'jquery',
            'emoticons'
        ], factory);
    } else if (typeof exports === "object") {
        module.exports = factory(require('jquery'), require('emoticons'));
    } else {
        factory(
            root.jQuery,
            root.emoticons
        );
    }
}(this, function ($, emoticons, undefined) {
    'use strict';
    var $emoticons = $.emoticons = $.fn.emoticons = function () {
        if (typeof this !== 'object' || this.selector === undefined) {
            throw "Use with jQuery Selector";
        }
        if (arguments.length > 0) {
            $emoticons.inputOptions = arguments[0];
        } else {
            $emoticons.inputOptions = {};
        }
        $emoticons.options = $.extend({}, $emoticons.defaultOptions, $emoticons.inputOptions);

        $emoticons.$replace = function ($copy) {
            var excluded = $copy.not($emoticons.options.exclude);

            excluded.each(function () {
                var container = $(this);
                var replacedHtml = emoticons(container.html(), $emoticons.options);
                if (container.html() !== replacedHtml) {
                    container.html(replacedHtml);
                };
            });
            if ($emoticons.options.delay > 0) {
                setTimeout(function () {
                    $emoticons.$replace($($copy.selector));
                }, $emoticons.options.delay);
            };
        };
        $emoticons.$replace(this);

        return this;
    };
    $emoticons.defaultOptions = { delay: 0, exclude: 'pre,code,.no-emoticons' }
}));