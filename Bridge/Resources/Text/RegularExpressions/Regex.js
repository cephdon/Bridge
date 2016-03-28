﻿// @source Text/RegularExpressions/Regex.js

Bridge.define("Bridge.Text.RegularExpressions.Regex", {
    statics: {
        _cacheSize: 15,
        _defaultMatchTimeout: Bridge.TimeSpan.fromMilliseconds(-1),

        getCacheSize: function () {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass._cacheSize;
        },

        setCacheSize: function (value) {
            if (value < 0) {
                throw new Bridge.ArgumentOutOfRangeException("value");
            }

            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            regexClass._cacheSize = value;
            //TODO: remove extra items from cache
        },

        escape: function (str) {
            if (str == null) {
                throw new Bridge.ArgumentNullException("str");
            }

            var scope = Bridge.Text.RegularExpressions;
            var regexParserClass = Bridge.get(scope.RegexParser);
            return regexParserClass.escape(str);
        },

        unescape: function (str) {
            if (str == null) {
                throw new Bridge.ArgumentNullException("str");
            }

            var scope = Bridge.Text.RegularExpressions;
            var regexParserClass = Bridge.get(scope.RegexParser);
            return regexParserClass.unescape(str);
        },

        isMatch: function (input, pattern) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.isMatch$2(input, pattern, scope.RegexOptions.None, regexClass._defaultMatchTimeout);
        },

        isMatch$1: function (input, pattern, options) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.isMatch$2(input, pattern, options, regexClass._defaultMatchTimeout);
        },

        isMatch$2: function (input, pattern, options, matchTimeout) {
            var scope = Bridge.Text.RegularExpressions;
            var regex = new scope.Regex("constructor$3", pattern, options, matchTimeout, true);
            return regex.isMatch(input);
        },

        match: function (input, pattern) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.match$2(input, pattern, scope.RegexOptions.None, regexClass._defaultMatchTimeout);
        },

        match$1: function (input, pattern, options) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.match$2(input, pattern, options, regexClass._defaultMatchTimeout);
        },

        match$2: function (input, pattern, options, matchTimeout) {
            var scope = Bridge.Text.RegularExpressions;
            var regex = new scope.Regex("constructor$3", pattern, options, matchTimeout, true);
            return regex.match(input);
        },

        matches: function (input, pattern) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.matches$2(input, pattern, scope.RegexOptions.None, regexClass._defaultMatchTimeout);
        },

        matches$1: function (input, pattern, options) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.matches$2(input, pattern, options, regexClass._defaultMatchTimeout);
        },

        matches$2: function (input, pattern, options, matchTimeout) {
            var scope = Bridge.Text.RegularExpressions;
            var regex = new scope.Regex("constructor$3", pattern, options, matchTimeout, true);
            return regex.matches(input);
        },

        replace: function (input, pattern, replacement) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.replace$2(input, pattern, replacement, scope.RegexOptions.None, regexClass._defaultMatchTimeout);
        },

        replace$1: function (input, pattern, replacement, options) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.replace$2(input, pattern, replacement, options, regexClass._defaultMatchTimeout);
        },

        replace$2: function (input, pattern, replacement, options, matchTimeout) {
            var scope = Bridge.Text.RegularExpressions;
            var regex = new scope.Regex("constructor$3", pattern, options, matchTimeout, true);
            return regex.replace(input, replacement);
        },

        replace$3: function (input, pattern, evaluator) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.replace$5(input, pattern, evaluator, scope.RegexOptions.None, regexClass._defaultMatchTimeout);
        },

        replace$4: function (input, pattern, evaluator, options) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.replace$5(input, pattern, evaluator, options, regexClass._defaultMatchTimeout);
        },

        replace$5: function (input, pattern, evaluator, options, matchTimeout) {
            var scope = Bridge.Text.RegularExpressions;
            var regex = new scope.Regex("constructor$3", pattern, options, matchTimeout, true);
            return regex.replace$3(input, evaluator);
        },

        split: function (input, pattern) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.split$2(input, pattern, scope.RegexOptions.None, regexClass._defaultMatchTimeout);
        },

        split$1: function (input, pattern, options) {
            var scope = Bridge.Text.RegularExpressions;
            var regexClass = Bridge.get(scope.Regex);
            return regexClass.split$2(input, pattern, options, regexClass._defaultMatchTimeout);
        },

        split$2: function (input, pattern, options, matchTimeout) {
            var scope = Bridge.Text.RegularExpressions;
            var regex = new scope.Regex("constructor$3", pattern, options, matchTimeout, true);
            return regex.split(input);
        }
    },

    _pattern: "",
    _matchTimeout: Bridge.TimeSpan.fromMilliseconds(-1),
    _runner: null,
    _caps: null,
    _capsize: 0,
    _capnames: null,
    _capslist: null,

    config: {
        init: function() {
            this._options = Bridge.Text.RegularExpressions.RegexOptions.None;
        }
    },

    constructor: function (pattern) {
        this.constructor$1(pattern, Bridge.Text.RegularExpressions.RegexOptions.None);
    },

    constructor$1: function (pattern, options) {
        this.constructor$2(pattern, options, Bridge.TimeSpan.fromMilliseconds(-1));
    },

    constructor$2: function (pattern, options, matchTimeout) {
        this.constructor$3(pattern, options, matchTimeout, false);
    },

    constructor$3: function (pattern, options, matchTimeout, useCache) {
        var scope = Bridge.Text.RegularExpressions;

        if (pattern == null) {
            throw new Bridge.ArgumentNullException("pattern");
        }

        if (options < scope.RegexOptions.None || ((options >> 10) !== 0)) {
            throw new Bridge.ArgumentOutOfRangeException("options");
        }

        if (((options & scope.RegexOptions.ECMAScript) !== 0)
            && ((options & ~(scope.RegexOptions.ECMAScript |
                    scope.RegexOptions.IgnoreCase |
                    scope.RegexOptions.Multiline |
                    scope.RegexOptions.CultureInvariant
            )) !== 0)) {
            throw new Bridge.ArgumentOutOfRangeException("options");
        }

        this._pattern = pattern;
        this._options = options;
        this._matchTimeout = matchTimeout;
        this._runner = new scope.RegexRunner();


        //TODO: cache
        var engine = Bridge.get(Bridge.Text.RegularExpressions.RegexNetEngine);
        var groupInfos = engine.parsePatternGroups(this._pattern);

        this._capsize = groupInfos.length;
        //this._caps = ;/
        this._capslist = [];
        this._capnames = {};

        this._capsize ++;
        this._capslist.push("0");
        this._capnames["0"] = 0;

        var i;
        var groupInfo;

        for (i = 0; i < groupInfos.length; i++) {
            groupInfo = groupInfos[i];
            if (!groupInfo.hasName) {
                this._capslist.push(groupInfo.name);
                this._capnames[groupInfo.name] = this._capslist.length-1;
            }
        }
        for (i = 0; i < groupInfos.length; i++) {
            groupInfo = groupInfos[i];
            if (groupInfo.hasName) {
                this._capslist.push(groupInfo.name);
                this._capnames[groupInfo.name] = this._capslist.length - 1;
            }
        }

        //TODO: ValidateMatchTimeout(matchTimeout);
    },

    getMatchTimeout: function () {
        return this._matchTimeout;
    },

    getOptions: function () {
        return this._options;
    },

    getRightToLeft: function () {
        return (this._options & Bridge.Text.RegularExpressions.RegexOptions.RightToLeft) !== 0;
    },

    isMatch: function (input) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? input.length : 0;
        return this.isMatch$1(input, startat);
    },

    isMatch$1: function (input, startat) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }


        var match = this._runner.run(this, true, -1, input, 0, input.length, startat);
        return match == null;
    },

    match: function (input) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? input.length : 0;
        return this.match$1(input, startat);
    },

    match$1: function (input, startat) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        return this._runner.run(this, false, -1, input, 0, input.length, startat);
    },

    match$2: function (input, beginning, length) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? beginning + length : beginning;
        return this._runner.run(this, false, -1, input, beginning, length, startat);
    },

    matches: function (input) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? input.length : 0;
        return this.matches$1(input, startat);
    },

    matches$1: function (input, startat) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        return new Bridge.Text.RegularExpressions.MatchCollection(this, input, 0, input.length, startat);
    },

    getGroupNames: function () {
        if (this._capslist == null) {
            var invariantCulture = Bridge.get(Bridge.CultureInfo).invariantCulture;

            var result = [];
            var max = this._capsize;
            for (var i = 0; i < max; i++) {
                result[i] = Bridge.Convert.toString(i, invariantCulture, Bridge.Convert.typeCodes.Int32);
            }

            return result;
        }
        else {
            return this._capslist.slice();
        }
    },

    getGroupNumbers: function () {
        var result;
        var caps = this._caps;
 
        if (caps == null) {
            result = [];
            var max = this._capsize;
            for (var i = 0; i < max; i++) {
                result.push(i);
            }
        }
        else {
            result = [];
            for (var key in caps) {
                if(caps.hasOwnProperty(key)) {
                    result[caps[key]] = key;
                }
            }
        }
 
        return result;
    },

    groupNameFromNumber: function(i) {

        if (this._capslist == null) {
            if (i >= 0 && i < this._capsize) {
                var invariantCulture = Bridge.get(Bridge.CultureInfo).invariantCulture;
                return Bridge.Convert.toString(i, invariantCulture, Bridge.Convert.typeCodes.Int32);
            }
            return "";

        } else {

            if (this._caps != null) {
                var obj = this._caps[i];
                if (obj == null) {
                    return "";
                }
                return parseInt(obj);
            }

            if (i >= 0 && i < this._capslist.length) {
                return this._capslist[i];
            }

            return "";
        }
    },

    groupNumberFromName: function(name) {
        if (name == null) {
            throw new Bridge.ArgumentNullException("name");
        }

        // look up name if we have a hashtable of names
        if (this._capnames != null) {
            var ret = this._capnames[name];
            if (ret == null) {
                return -1;
            }
            return parseInt(ret);
        }

        // convert to an int if it looks like a number
        var result = 0;
        for (var i = 0; i < name.Length; i++) {
            var ch = name[i];

            if (ch > "9" || ch < "0") {
                return -1;
            }

            result *= 10;
            result += (ch - "0");
        }

        // return int if it's in range
        if (result >= 0 && result < this._capsize) {
            return result;
        }

        return -1;
    },

    replace: function (input, replacement) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? input.length : 0;
        return this.replace$2(input, replacement, -1, startat);
    },

    replace$1: function (input, replacement, count) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? input.length : 0;
        return this.replace$2(input, replacement, count, startat);
    },

    replace$2: function (input, replacement, count, startat) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }
        if (replacement == null) {
            throw new Bridge.ArgumentNullException("replacement");
        }

        var regexParser = Bridge.get(Bridge.Text.RegularExpressions.RegexParser);
        var repl = regexParser.parseReplacement(replacement, this._caps, this._capsize, this._capnames, this._options);
        //TODO: Cache

        return repl.replace(this, input, count, startat);
    },

    replace$3: function (input, evaluator) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? input.length : 0;
        return this.replace$5(input, evaluator, -1, startat);
    },

    replace$4: function (input, evaluator, count) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? input.length : 0;
        return this.replace$5(input, evaluator, count, startat);
    },

    replace$5: function (input, evaluator, count, startat) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var scope = Bridge.Text.RegularExpressions;
        var regexReplacement = Bridge.get(scope.RegexReplacement);
        return regexReplacement.replace(evaluator, this, input, count, startat);
    },

    split: function (input) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? input.length : 0;
        return this.split$2(input, 0, startat);
    },

    split$1: function (input, count) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var startat = this.getRightToLeft() ? input.length : 0;
        return this.split$2(input, count, startat);
    },

    split$2: function (input, count, startat) {
        if (input == null) {
            throw new Bridge.ArgumentNullException("input");
        }

        var scope = Bridge.Text.RegularExpressions;
        var regexReplacement = Bridge.get(scope.RegexReplacement);
        return regexReplacement.split(this, input, count, startat);
    }
});
