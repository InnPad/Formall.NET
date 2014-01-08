(function (undefined) {
    if (!Object.keys) {
        Object.keys = (function () {
            var hasOwnProperty = Object.prototype.hasOwnProperty,
                hasDontEnumBug = !({ toString: null }).propertyIsEnumerable('toString'),
                dontEnums = [
                  'toString',
                  'toLocaleString',
                  'valueOf',
                  'hasOwnProperty',
                  'isPrototypeOf',
                  'propertyIsEnumerable',
                  'constructor'
                ],
                dontEnumsLength = dontEnums.length;

            return function (obj) {
                if (typeof obj !== 'object' && typeof obj !== 'function' || obj === null) throw new TypeError('Object.keys called on non-object');

                var result = [];

                for (var prop in obj) {
                    if (hasOwnProperty.call(obj, prop)) result.push(prop);
                }

                if (hasDontEnumBug) {
                    for (var i = 0; i < dontEnumsLength; i++) {
                        if (hasOwnProperty.call(obj, dontEnums[i])) result.push(dontEnums[i]);
                    }
                }
                return result;
            };
        })();
    }
})();

(function (undefined) {
    if (!Object.values) {
        Object.values = (function () {
            return function (obj, keys) {
                if (typeof obj !== 'object' && typeof obj !== 'function' || obj === null) throw new TypeError('Object.values called on non-object');

                keys = keys || Object.keys(obj);

                var result = [];
                for (var i = 0, count = keys.length; i < count; i++) {
                    var key = keys[i];
                    var value = obj[key];
                    result.push(value);
                }
                return result;
            };
        })();
    }
})();

(function (undefined) {
    if (!Object.toDictionary) {
        Object.toDictionary = (function () {
            return function (obj, keys) {
                if (typeof obj !== 'object' && typeof obj !== 'function' || obj === null) throw new TypeError('Object.toDictionary called on non-object');

                keys = keys || Object.keys(obj);

                var result = [];
                for (var i = 0, count = keys.length; i < count; i++) {
                    var key = keys[i];
                    var value = obj[key];
                    result.push({ key: key, value: value });
                }
                return result;
            };
        })();
    }
})();


var model = {
    "Fields": {
    },
    "Title": {
    },
    "Summary": {
    }
};

var node = {
    "#": {
    },
    "@": {
    },
    "+": {
    },
    "-": {
    },
    "=": {
    }
};

var keys = Object.keys(node);
var values = Object.values(node);
var dictionary = Object.toDictionary(node);

keys = Object.keys(model);
values = Object.values(model);
dictionary = Object.toDictionary(model);

