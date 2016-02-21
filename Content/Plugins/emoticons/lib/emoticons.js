(function (root, factory) {
    "use strict";
    if (typeof define === "function" && define.amd) {
        define([], factory);
    } else if (typeof exports === "object") {
        module.exports = factory();
    } else {
        root.emoticons = factory();
    }
}(this, function(undefined) {
    var emoticonsArray,
    codesMap = {},
    primaryCodesMap = {},
    regexp,
    metachars = /[[\]{}()*+?.\\|^$\-,&#\s]/g,
        fn = {};
    var entityMap = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#39;',
        '/': '&#x2F;'
    };

    function escape(string) {
        return String(string).replace(/[&<>"'\/]/g, function (s) {
            return entityMap[s];
        });
    }

    fn.define = function (data) {
        var name,
            i,
            codes,
            code,
            patterns = [];

        for (name in data) {
            if (data.hasOwnProperty(name)) {
                codes = data[name].codes;
                for (i in codes) {
                    if (codes.hasOwnProperty(i)) {
                        code = codes[i];
                        codesMap[code] = name;
                        codesMap[escape(code)] = name;
                        if (parseInt(i) === 0) {
                            primaryCodesMap[code] = name;
                        }
                    }
                }
            }
        }

        for (code in codesMap) {
            if (codesMap.hasOwnProperty(code)) {
                patterns.push('(' + code.replace(metachars, "\\$&") + ')');
            }
        }

        regexp = new RegExp(patterns.join('|'), 'g');
        emoticonsArray = data;
    };

    fn.replace = function (text, func) {
        return text.replace(regexp, function (code) {
            var name = codesMap[code];
            return (func || fn.defaultTemplate)(name, code, emoticonsArray[name].title);
        });
    };

    fn.toString = function (func) {
        var code,
            str = '',
            name;

        for (code in primaryCodesMap) {
            if (primaryCodesMap.hasOwnProperty(code)) {
                name = primaryCodesMap[code];
                str += (func || fn.defaultTemplate)(name, code, emoticonsArray[name].title);
            }
        }

        return str;
    };

    fn.defaultTemplate = function (name, code, title) {
        return '<span class="emoticon emoticon-' + name + '" title="' + title + '"> ' +
            code.split('').join(' ') + ' </span>';
    };

    var emoticons = function emoticonsContructer() {
        if (arguments.length < 1) {
            throw "Arguments required";
        }
        if (arguments.length > 0 && typeof arguments[0] === 'string') {
            emoticons.inputText = arguments[0];
            if (arguments.length > 1) {
                emoticons.inputOptions = arguments[1];
            } else {
                emoticons.inputOptions = {};
            }
        } else {
            throw "First argument type must string";
        }
        emoticons.options = $.extend({}, emoticons.defaultOptions, emoticons.inputOptions);
        if (emoticons.definitions.hasOwnProperty(emoticons.options.type) === false) {
            throw "Unknown definition type";
        }
        if (typeof emoticons.options.template !== 'function') {
            throw "Template type must function";
        }
        fn.define(emoticons.definitions[emoticons.options.type]);
        return fn.replace(emoticons.inputText, emoticons.options.template);
    };
    emoticons.fn = fn;
    emoticons.definitions = {};
    emoticons.definitions['skype'] = { smile: { title: "Smile", codes: [":)", ":=)", ":-)"] }, "sad-smile": { title: "Sad Smile", codes: [":(", ":=(", ":-("] }, "big-smile": { title: "Big Smile", codes: [":D", ":=D", ":-D", ":d", ":=d", ":-d"] }, cool: { title: "Cool", codes: ["8)", "8=)", "8-)", "B)", "B=)", "B-)", "(cool)"] }, wink: { title: "Wink", codes: [":o", ":=o", ":-o", ":O", ":=O", ":-O"] }, crying: { title: "Crying", codes: [";(", ";-(", ";=("] }, sweating: { title: "Sweating", codes: ["(sweat)", "(:|"] }, speechless: { title: "Speechless", codes: [":|", ":=|", ":-|"] }, kiss: { title: "Kiss", codes: [":*", ":=*", ":-*"] }, "tongue-out": { title: "Tongue Out", codes: [":P", ":=P", ":-P", ":p", ":=p", ":-p"] }, blush: { title: "Blush", codes: ["(blush)", ":$", ":-$", ":=$", ':">'] }, wondering: { title: "Wondering", codes: [":^)"] }, sleepy: { title: "Sleepy", codes: ["|-)", "I-)", "I=)", "(snooze)"] }, dull: { title: "Dull", codes: ["|(", "|-(", "|=("] }, "in-love": { title: "In love", codes: ["(inlove)"] }, "evil-grin": { title: "Evil grin", codes: ["]:)", ">:)", "(grin)"] }, talking: { title: "Talking", codes: ["(talk)"] }, yawn: { title: "Yawn", codes: ["(yawn)", "|-()"] }, puke: { title: "Puke", codes: ["(puke)", ":&", ":-&", ":=&"] }, "doh!": { title: "Doh!", codes: ["(doh)"] }, angry: { title: "Angry", codes: [":@", ":-@", ":=@", "x(", "x-(", "x=(", "X(", "X-(", "X=("] }, "it-wasnt-me": { title: "It wasn't me", codes: ["(wasntme)"] }, party: { title: "Party!!!", codes: ["(party)"] }, worried: { title: "Worried", codes: [":S", ":-S", ":=S", ":s", ":-s", ":=s"] }, mmm: { title: "Mmm...", codes: ["(mm)"] }, nerd: { title: "Nerd", codes: ["8-|", "B-|", "8|", "B|", "8=|", "B=|", "(nerd)"] }, "lips-sealed": { title: "Lips Sealed", codes: [":x", ":-x", ":X", ":-X", ":#", ":-#", ":=x", ":=X", ":=#"] }, hi: { title: "Hi", codes: ["(hi)"] }, call: { title: "Call", codes: ["(call)"] }, devil: { title: "Devil", codes: ["(devil)"] }, angel: { title: "Angel", codes: ["(angel)"] }, envy: { title: "Envy", codes: ["(envy)"] }, wait: { title: "Wait", codes: ["(wait)"] }, bear: { title: "Bear", codes: ["(bear)", "(hug)"] }, "make-up": { title: "Make-up", codes: ["(makeup)", "(kate)"] }, "covered-laugh": { title: "Covered Laugh", codes: ["(giggle)", "(chuckle)"] }, "clapping-hands": { title: "Clapping Hands", codes: ["(clap)"] }, thinking: { title: "Thinking", codes: ["(think)", ":?", ":-?", ":=?"] }, bow: { title: "Bow", codes: ["(bow)"] }, rofl: { title: "Rolling on the floor laughing", codes: ["(rofl)"] }, whew: { title: "Whew", codes: ["(whew)"] }, happy: { title: "Happy", codes: ["(happy)"] }, smirking: { title: "Smirking", codes: ["(smirk)"] }, nodding: { title: "Nodding", codes: ["(nod)"] }, shaking: { title: "Shaking", codes: ["(shake)"] }, punch: { title: "Punch", codes: ["(punch)"] }, emo: { title: "Emo", codes: ["(emo)"] }, yes: { title: "Yes", codes: ["(y)", "(Y)", "(ok)"] }, no: { title: "No", codes: ["(n)", "(N)"] }, handshake: { title: "Shaking Hands", codes: ["(handshake)"] }, skype: { title: "Skype", codes: ["(skype)", "(ss)"] }, heart: { title: "Heart", codes: ["(h)", "<3", "(H)", "(l)", "(L)"] }, "broken-heart": { title: "Broken heart", codes: ["(u)", "(U)"] }, mail: { title: "Mail", codes: ["(e)", "(m)"] }, flower: { title: "Flower", codes: ["(f)", "(F)"] }, rain: { title: "Rain", codes: ["(rain)", "(london)", "(st)"] }, sun: { title: "Sun", codes: ["(sun)"] }, time: { title: "Time", codes: ["(o)", "(O)", "(time)"] }, music: { title: "Music", codes: ["(music)"] }, movie: { title: "Movie", codes: ["(~)", "(film)", "(movie)"] }, phone: { title: "Phone", codes: ["(mp)", "(ph)"] }, coffee: { title: "Coffee", codes: ["(coffee)"] }, pizza: { title: "Pizza", codes: ["(pizza)", "(pi)"] }, cash: { title: "Cash", codes: ["(cash)", "(mo)", "($)"] }, muscle: { title: "Muscle", codes: ["(muscle)", "(flex)"] }, cake: { title: "Cake", codes: ["(^)", "(cake)"] }, beer: { title: "Beer", codes: ["(beer)"] }, drink: { title: "Drink", codes: ["(d)", "(D)"] }, dance: { title: "Dance", codes: ["(dance)", "\\o/", "\\:D/", "\\:d/"] }, ninja: { title: "Ninja", codes: ["(ninja)"] }, star: { title: "Star", codes: ["(*)"] }, mooning: { title: "Mooning", codes: ["(mooning)"] }, finger: { title: "Finger", codes: ["(finger)"] }, bandit: { title: "Bandit", codes: ["(bandit)"] }, drunk: { title: "Drunk", codes: ["(drunk)"] }, smoking: { title: "Smoking", codes: ["(smoking)", "(smoke)", "(ci)"] }, toivo: { title: "Toivo", codes: ["(toivo)"] }, rock: { title: "Rock", codes: ["(rock)"] }, headbang: { title: "Headbang", codes: ["(headbang)", "(banghead)"] }, bug: { title: "Bug", codes: ["(bug)"] }, fubar: { title: "Fubar", codes: ["(fubar)"] }, poolparty: { title: "Poolparty", codes: ["(poolparty)"] }, swearing: { title: "Swearing", codes: ["(swear)"] }, tmi: { title: "TMI", codes: ["(tmi)"] }, heidy: { title: "Heidy", codes: ["(heidy)"] }, myspace: { title: "MySpace", codes: ["(MySpace)"] }, malthe: { title: "Malthe", codes: ["(malthe)"] }, tauri: { title: "Tauri", codes: ["(tauri)"] }, priidu: { title: "Priidu", codes: ["(priidu)"] } };

    emoticons.defaultOptions = { type: 'skype', template: emoticons.fn.defaultTemplate };
    return emoticons;
}));