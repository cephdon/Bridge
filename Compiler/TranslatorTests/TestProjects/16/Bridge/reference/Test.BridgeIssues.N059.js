/**
 * @compiler Bridge.NET 16.3.1
 */
Bridge.assembly("TestProject", function ($asm, globals) {
    "use strict";

    Bridge.define("Test.BridgeIssues.N059.Class59", {
        statics: {
            methods: {
                Method1: function () { }
            }
        },
        methods: {
            Method1: function (d) { }
        }
    });

    Bridge.define("Test.BridgeIssues.N059.Class59.Aux1");
});
