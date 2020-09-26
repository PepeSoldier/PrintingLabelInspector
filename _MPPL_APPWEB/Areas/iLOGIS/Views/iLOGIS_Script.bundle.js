var iLogis =
/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ({

/***/ "./Areas/iLOGIS/Views/StockUnit/Stock.js":
/*!***********************************************!*\
  !*** ./Areas/iLOGIS/Views/StockUnit/Stock.js ***!
  \***********************************************/
/*! exports provided: Stock */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Stock", function() { return Stock; });
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var Stock = /*#__PURE__*/function () {
  function Stock() {
    _classCallCheck(this, Stock);
  }

  _createClass(Stock, [{
    key: "Test",
    value: function Test() {
      console.log("Stock :)");
    }
  }, {
    key: "Refresh",
    value: function Refresh() {
      console.log("Stock.Refresh.3");
    }
  }, {
    key: "TakaOfunkcja",
    value: function TakaOfunkcja() {
      console.log("co tu sie odpierdala");
    }
  }]);

  return Stock;
}();

/***/ }),

/***/ "./Areas/iLOGIS/Views/StockUnit/StockMobile.js":
/*!*****************************************************!*\
  !*** ./Areas/iLOGIS/Views/StockUnit/StockMobile.js ***!
  \*****************************************************/
/*! exports provided: StockMobile */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "StockMobile", function() { return StockMobile; });
/* harmony import */ var _Stock__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./Stock */ "./Areas/iLOGIS/Views/StockUnit/Stock.js");
function _typeof(obj) { "@babel/helpers - typeof"; if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _get(target, property, receiver) { if (typeof Reflect !== "undefined" && Reflect.get) { _get = Reflect.get; } else { _get = function _get(target, property, receiver) { var base = _superPropBase(target, property); if (!base) return; var desc = Object.getOwnPropertyDescriptor(base, property); if (desc.get) { return desc.get.call(receiver); } return desc.value; }; } return _get(target, property, receiver || target); }

function _superPropBase(object, property) { while (!Object.prototype.hasOwnProperty.call(object, property)) { object = _getPrototypeOf(object); if (object === null) break; } return object; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

function _createSuper(Derived) { return function () { var Super = _getPrototypeOf(Derived), result; if (_isNativeReflectConstruct()) { var NewTarget = _getPrototypeOf(this).constructor; result = Reflect.construct(Super, arguments, NewTarget); } else { result = Super.apply(this, arguments); } return _possibleConstructorReturn(this, result); }; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _isNativeReflectConstruct() { if (typeof Reflect === "undefined" || !Reflect.construct) return false; if (Reflect.construct.sham) return false; if (typeof Proxy === "function") return true; try { Date.prototype.toString.call(Reflect.construct(Date, [], function () {})); return true; } catch (e) { return false; } }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }


var StockMobile = /*#__PURE__*/function (_Stock) {
  _inherits(StockMobile, _Stock);

  var _super = _createSuper(StockMobile);

  function StockMobile() {
    _classCallCheck(this, StockMobile);

    return _super.apply(this, arguments);
  }

  _createClass(StockMobile, [{
    key: "Test",
    value: function Test() {
      _get(_getPrototypeOf(StockMobile.prototype), "Refresh", this).call(this);
    }
  }, {
    key: "CoSieOdjaniepawla",
    value: function CoSieOdjaniepawla() {
      console.log("CoSieOdjaniepawla");
    }
  }, {
    key: "Tusk",
    value: function Tusk() {
      var kaczynski = 2;
      var pawlowicz = 3;
      var zlypis = kaczynski + pawlowicz;
      return zlypis * 2;
    }
  }]);

  return StockMobile;
}(_Stock__WEBPACK_IMPORTED_MODULE_0__["Stock"]);

/***/ }),

/***/ "./Areas/iLOGIS/Views/iLOGIS_Script.js":
/*!*********************************************!*\
  !*** ./Areas/iLOGIS/Views/iLOGIS_Script.js ***!
  \*********************************************/
/*! exports provided: Stock, StockMobile */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _StockUnit_Stock__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./StockUnit/Stock */ "./Areas/iLOGIS/Views/StockUnit/Stock.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Stock", function() { return _StockUnit_Stock__WEBPACK_IMPORTED_MODULE_0__["Stock"]; });

/* harmony import */ var _StockUnit_StockMobile__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./StockUnit/StockMobile */ "./Areas/iLOGIS/Views/StockUnit/StockMobile.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "StockMobile", function() { return _StockUnit_StockMobile__WEBPACK_IMPORTED_MODULE_1__["StockMobile"]; });


 //import { StocksGrid } from "./StockUnit/StockGrid";
//import { StockLocationDetailsGrid } from "./StockUnit/StockLocationDetailsGrid";
//import { StockWarehouseDetailsGrid } from "./StockUnit/StockWarehouseDetailsGrid";
//let test = () => console.log("jebacz kaczora");
//var s = new Stock();
//s.Test();

//console.log("to tu sie....");
//console.log("to tu sie.... 2"); //var sm = new StockMobile();
//sm.Test();



/***/ }),

/***/ 0:
/*!***************************************************!*\
  !*** multi ./Areas/iLOGIS/Views/iLOGIS_Script.js ***!
  \***************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! ./Areas/iLOGIS/Views/iLOGIS_Script.js */"./Areas/iLOGIS/Views/iLOGIS_Script.js");


/***/ })

/******/ });
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9pTG9naXMvd2VicGFjay9ib290c3RyYXAiLCJ3ZWJwYWNrOi8vaUxvZ2lzLy4vQXJlYXMvaUxPR0lTL1ZpZXdzL1N0b2NrVW5pdC9TdG9jay5qcyIsIndlYnBhY2s6Ly9pTG9naXMvLi9BcmVhcy9pTE9HSVMvVmlld3MvU3RvY2tVbml0L1N0b2NrTW9iaWxlLmpzIiwid2VicGFjazovL2lMb2dpcy8uL0FyZWFzL2lMT0dJUy9WaWV3cy9pTE9HSVNfU2NyaXB0LmpzIl0sIm5hbWVzIjpbIlN0b2NrIiwiY29uc29sZSIsImxvZyIsIlN0b2NrTW9iaWxlIiwia2Fjenluc2tpIiwicGF3bG93aWN6IiwiamViYW55cGlzIl0sIm1hcHBpbmdzIjoiOztRQUFBO1FBQ0E7O1FBRUE7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTs7UUFFQTtRQUNBOztRQUVBO1FBQ0E7O1FBRUE7UUFDQTtRQUNBOzs7UUFHQTtRQUNBOztRQUVBO1FBQ0E7O1FBRUE7UUFDQTtRQUNBO1FBQ0EsMENBQTBDLGdDQUFnQztRQUMxRTtRQUNBOztRQUVBO1FBQ0E7UUFDQTtRQUNBLHdEQUF3RCxrQkFBa0I7UUFDMUU7UUFDQSxpREFBaUQsY0FBYztRQUMvRDs7UUFFQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0EseUNBQXlDLGlDQUFpQztRQUMxRSxnSEFBZ0gsbUJBQW1CLEVBQUU7UUFDckk7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7UUFDQSwyQkFBMkIsMEJBQTBCLEVBQUU7UUFDdkQsaUNBQWlDLGVBQWU7UUFDaEQ7UUFDQTtRQUNBOztRQUVBO1FBQ0Esc0RBQXNELCtEQUErRDs7UUFFckg7UUFDQTs7O1FBR0E7UUFDQTs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7O0FDaEZPLElBQU1BLEtBQWI7QUFBQTtBQUFBO0FBQUE7O0FBQUE7QUFBQTtBQUFBLDJCQUdXO0FBQ0hDLGFBQU8sQ0FBQ0MsR0FBUixDQUFZLFVBQVo7QUFDSDtBQUxMO0FBQUE7QUFBQSw4QkFPYztBQUNORCxhQUFPLENBQUNDLEdBQVIsQ0FBWSxpQkFBWjtBQUNIO0FBVEw7QUFBQTtBQUFBLG1DQVdtQjtBQUNYRCxhQUFPLENBQUNDLEdBQVIsQ0FBWSxzQkFBWjtBQUNIO0FBYkw7O0FBQUE7QUFBQSxJOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7OztBQ0ZBO0FBRU8sSUFBTUMsV0FBYjtBQUFBOztBQUFBOztBQUFBO0FBQUE7O0FBQUE7QUFBQTs7QUFBQTtBQUFBO0FBQUEsMkJBRVE7QUFDTjtBQUNBO0FBSkY7QUFBQTtBQUFBLHdDQU1xQjtBQUNuQkYsYUFBTyxDQUFDQyxHQUFSLENBQVksbUJBQVo7QUFDQTtBQVJGO0FBQUE7QUFBQSwyQkFVUTtBQUNOLFVBQUlFLFNBQVMsR0FBRyxDQUFoQjtBQUNBLFVBQUlDLFNBQVMsR0FBRyxDQUFoQjtBQUNBLFVBQUlDLFNBQVMsR0FBR0YsU0FBUyxHQUFHQyxTQUE1QjtBQUNBLGFBQU9DLFNBQVMsR0FBRyxDQUFuQjtBQUNBO0FBZkY7O0FBQUE7QUFBQSxFQUFpQ04sNENBQWpDLEU7Ozs7Ozs7Ozs7OztBQ0ZBO0FBQUE7QUFBQTtBQUFBO0FBQUE7QUFBQTtBQUFBO0FBQUE7Q0FFQTtBQUNBO0FBQ0E7QUFFQTtBQUVBO0FBQ0E7O0FBRUFDLE9BQU8sQ0FBQ0MsR0FBUixDQUFZLGVBQVo7QUFDQUQsT0FBTyxDQUFDQyxHQUFSLENBQVksaUJBQVosRSxDQUVBO0FBQ0EiLCJmaWxlIjoiaUxPR0lTX1NjcmlwdC5idW5kbGUuanMiLCJzb3VyY2VzQ29udGVudCI6WyIgXHQvLyBUaGUgbW9kdWxlIGNhY2hlXG4gXHR2YXIgaW5zdGFsbGVkTW9kdWxlcyA9IHt9O1xuXG4gXHQvLyBUaGUgcmVxdWlyZSBmdW5jdGlvblxuIFx0ZnVuY3Rpb24gX193ZWJwYWNrX3JlcXVpcmVfXyhtb2R1bGVJZCkge1xuXG4gXHRcdC8vIENoZWNrIGlmIG1vZHVsZSBpcyBpbiBjYWNoZVxuIFx0XHRpZihpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXSkge1xuIFx0XHRcdHJldHVybiBpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXS5leHBvcnRzO1xuIFx0XHR9XG4gXHRcdC8vIENyZWF0ZSBhIG5ldyBtb2R1bGUgKGFuZCBwdXQgaXQgaW50byB0aGUgY2FjaGUpXG4gXHRcdHZhciBtb2R1bGUgPSBpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXSA9IHtcbiBcdFx0XHRpOiBtb2R1bGVJZCxcbiBcdFx0XHRsOiBmYWxzZSxcbiBcdFx0XHRleHBvcnRzOiB7fVxuIFx0XHR9O1xuXG4gXHRcdC8vIEV4ZWN1dGUgdGhlIG1vZHVsZSBmdW5jdGlvblxuIFx0XHRtb2R1bGVzW21vZHVsZUlkXS5jYWxsKG1vZHVsZS5leHBvcnRzLCBtb2R1bGUsIG1vZHVsZS5leHBvcnRzLCBfX3dlYnBhY2tfcmVxdWlyZV9fKTtcblxuIFx0XHQvLyBGbGFnIHRoZSBtb2R1bGUgYXMgbG9hZGVkXG4gXHRcdG1vZHVsZS5sID0gdHJ1ZTtcblxuIFx0XHQvLyBSZXR1cm4gdGhlIGV4cG9ydHMgb2YgdGhlIG1vZHVsZVxuIFx0XHRyZXR1cm4gbW9kdWxlLmV4cG9ydHM7XG4gXHR9XG5cblxuIFx0Ly8gZXhwb3NlIHRoZSBtb2R1bGVzIG9iamVjdCAoX193ZWJwYWNrX21vZHVsZXNfXylcbiBcdF9fd2VicGFja19yZXF1aXJlX18ubSA9IG1vZHVsZXM7XG5cbiBcdC8vIGV4cG9zZSB0aGUgbW9kdWxlIGNhY2hlXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLmMgPSBpbnN0YWxsZWRNb2R1bGVzO1xuXG4gXHQvLyBkZWZpbmUgZ2V0dGVyIGZ1bmN0aW9uIGZvciBoYXJtb255IGV4cG9ydHNcbiBcdF9fd2VicGFja19yZXF1aXJlX18uZCA9IGZ1bmN0aW9uKGV4cG9ydHMsIG5hbWUsIGdldHRlcikge1xuIFx0XHRpZighX193ZWJwYWNrX3JlcXVpcmVfXy5vKGV4cG9ydHMsIG5hbWUpKSB7XG4gXHRcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIG5hbWUsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBnZXR0ZXIgfSk7XG4gXHRcdH1cbiBcdH07XG5cbiBcdC8vIGRlZmluZSBfX2VzTW9kdWxlIG9uIGV4cG9ydHNcbiBcdF9fd2VicGFja19yZXF1aXJlX18uciA9IGZ1bmN0aW9uKGV4cG9ydHMpIHtcbiBcdFx0aWYodHlwZW9mIFN5bWJvbCAhPT0gJ3VuZGVmaW5lZCcgJiYgU3ltYm9sLnRvU3RyaW5nVGFnKSB7XG4gXHRcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFN5bWJvbC50b1N0cmluZ1RhZywgeyB2YWx1ZTogJ01vZHVsZScgfSk7XG4gXHRcdH1cbiBcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsICdfX2VzTW9kdWxlJywgeyB2YWx1ZTogdHJ1ZSB9KTtcbiBcdH07XG5cbiBcdC8vIGNyZWF0ZSBhIGZha2UgbmFtZXNwYWNlIG9iamVjdFxuIFx0Ly8gbW9kZSAmIDE6IHZhbHVlIGlzIGEgbW9kdWxlIGlkLCByZXF1aXJlIGl0XG4gXHQvLyBtb2RlICYgMjogbWVyZ2UgYWxsIHByb3BlcnRpZXMgb2YgdmFsdWUgaW50byB0aGUgbnNcbiBcdC8vIG1vZGUgJiA0OiByZXR1cm4gdmFsdWUgd2hlbiBhbHJlYWR5IG5zIG9iamVjdFxuIFx0Ly8gbW9kZSAmIDh8MTogYmVoYXZlIGxpa2UgcmVxdWlyZVxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy50ID0gZnVuY3Rpb24odmFsdWUsIG1vZGUpIHtcbiBcdFx0aWYobW9kZSAmIDEpIHZhbHVlID0gX193ZWJwYWNrX3JlcXVpcmVfXyh2YWx1ZSk7XG4gXHRcdGlmKG1vZGUgJiA4KSByZXR1cm4gdmFsdWU7XG4gXHRcdGlmKChtb2RlICYgNCkgJiYgdHlwZW9mIHZhbHVlID09PSAnb2JqZWN0JyAmJiB2YWx1ZSAmJiB2YWx1ZS5fX2VzTW9kdWxlKSByZXR1cm4gdmFsdWU7XG4gXHRcdHZhciBucyA9IE9iamVjdC5jcmVhdGUobnVsbCk7XG4gXHRcdF9fd2VicGFja19yZXF1aXJlX18ucihucyk7XG4gXHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShucywgJ2RlZmF1bHQnLCB7IGVudW1lcmFibGU6IHRydWUsIHZhbHVlOiB2YWx1ZSB9KTtcbiBcdFx0aWYobW9kZSAmIDIgJiYgdHlwZW9mIHZhbHVlICE9ICdzdHJpbmcnKSBmb3IodmFyIGtleSBpbiB2YWx1ZSkgX193ZWJwYWNrX3JlcXVpcmVfXy5kKG5zLCBrZXksIGZ1bmN0aW9uKGtleSkgeyByZXR1cm4gdmFsdWVba2V5XTsgfS5iaW5kKG51bGwsIGtleSkpO1xuIFx0XHRyZXR1cm4gbnM7XG4gXHR9O1xuXG4gXHQvLyBnZXREZWZhdWx0RXhwb3J0IGZ1bmN0aW9uIGZvciBjb21wYXRpYmlsaXR5IHdpdGggbm9uLWhhcm1vbnkgbW9kdWxlc1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5uID0gZnVuY3Rpb24obW9kdWxlKSB7XG4gXHRcdHZhciBnZXR0ZXIgPSBtb2R1bGUgJiYgbW9kdWxlLl9fZXNNb2R1bGUgP1xuIFx0XHRcdGZ1bmN0aW9uIGdldERlZmF1bHQoKSB7IHJldHVybiBtb2R1bGVbJ2RlZmF1bHQnXTsgfSA6XG4gXHRcdFx0ZnVuY3Rpb24gZ2V0TW9kdWxlRXhwb3J0cygpIHsgcmV0dXJuIG1vZHVsZTsgfTtcbiBcdFx0X193ZWJwYWNrX3JlcXVpcmVfXy5kKGdldHRlciwgJ2EnLCBnZXR0ZXIpO1xuIFx0XHRyZXR1cm4gZ2V0dGVyO1xuIFx0fTtcblxuIFx0Ly8gT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLm8gPSBmdW5jdGlvbihvYmplY3QsIHByb3BlcnR5KSB7IHJldHVybiBPYmplY3QucHJvdG90eXBlLmhhc093blByb3BlcnR5LmNhbGwob2JqZWN0LCBwcm9wZXJ0eSk7IH07XG5cbiBcdC8vIF9fd2VicGFja19wdWJsaWNfcGF0aF9fXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLnAgPSBcIlwiO1xuXG5cbiBcdC8vIExvYWQgZW50cnkgbW9kdWxlIGFuZCByZXR1cm4gZXhwb3J0c1xuIFx0cmV0dXJuIF9fd2VicGFja19yZXF1aXJlX18oX193ZWJwYWNrX3JlcXVpcmVfXy5zID0gMCk7XG4iLCJcclxuXHJcbmV4cG9ydCBjbGFzcyBTdG9jayB7XHJcblxyXG5cclxuICAgIFRlc3QoKSB7XHJcbiAgICAgICAgY29uc29sZS5sb2coXCJTdG9jayA6KVwiKTtcclxuICAgIH1cclxuXHJcbiAgICBSZWZyZXNoKCkge1xyXG4gICAgICAgIGNvbnNvbGUubG9nKFwiU3RvY2suUmVmcmVzaC4zXCIpO1xyXG4gICAgfVxyXG5cclxuICAgIFRha2FPZnVua2NqYSgpIHtcclxuICAgICAgICBjb25zb2xlLmxvZyhcImNvIHR1IHNpZSBvZHBpZXJkYWxhXCIpO1xyXG4gICAgfVxyXG5cclxufVxyXG4iLCJpbXBvcnQgeyBTdG9jayB9IGZyb20gXCIuL1N0b2NrXCI7XHJcblxyXG5leHBvcnQgY2xhc3MgU3RvY2tNb2JpbGUgZXh0ZW5kcyBTdG9jayB7XHJcblx0XHJcblx0VGVzdCgpIHtcclxuXHRcdHN1cGVyLlJlZnJlc2goKTtcclxuXHR9XHJcblxyXG5cdENvU2llT2RqYW5pZXBhd2xhKCkge1xyXG5cdFx0Y29uc29sZS5sb2coXCJDb1NpZU9kamFuaWVwYXdsYVwiKTtcclxuXHR9XHJcblxyXG5cdFR1c2soKSB7XHJcblx0XHRsZXQga2Fjenluc2tpID0gMjtcclxuXHRcdGxldCBwYXdsb3dpY3ogPSAzO1xyXG5cdFx0bGV0IGplYmFueXBpcyA9IGthY3p5bnNraSArIHBhd2xvd2ljejtcclxuXHRcdHJldHVybiBqZWJhbnlwaXMgKiAyO1xyXG5cdH1cclxuXHRcclxufSIsImltcG9ydCB7IFN0b2NrIH0gZnJvbSBcIi4vU3RvY2tVbml0L1N0b2NrXCI7XHJcbmltcG9ydCB7IFN0b2NrTW9iaWxlIH0gZnJvbSBcIi4vU3RvY2tVbml0L1N0b2NrTW9iaWxlXCI7XHJcbi8vaW1wb3J0IHsgU3RvY2tzR3JpZCB9IGZyb20gXCIuL1N0b2NrVW5pdC9TdG9ja0dyaWRcIjtcclxuLy9pbXBvcnQgeyBTdG9ja0xvY2F0aW9uRGV0YWlsc0dyaWQgfSBmcm9tIFwiLi9TdG9ja1VuaXQvU3RvY2tMb2NhdGlvbkRldGFpbHNHcmlkXCI7XHJcbi8vaW1wb3J0IHsgU3RvY2tXYXJlaG91c2VEZXRhaWxzR3JpZCB9IGZyb20gXCIuL1N0b2NrVW5pdC9TdG9ja1dhcmVob3VzZURldGFpbHNHcmlkXCI7XHJcblxyXG4vL2xldCB0ZXN0ID0gKCkgPT4gY29uc29sZS5sb2coXCJqZWJhY3oga2Fjem9yYVwiKTtcclxuXHJcbi8vdmFyIHMgPSBuZXcgU3RvY2soKTtcclxuLy9zLlRlc3QoKTtcclxuXHJcbmNvbnNvbGUubG9nKFwidG8gdHUgc2llLi4uLlwiKTtcclxuY29uc29sZS5sb2coXCJ0byB0dSBzaWUuLi4uIDJcIik7XHJcblxyXG4vL3ZhciBzbSA9IG5ldyBTdG9ja01vYmlsZSgpO1xyXG4vL3NtLlRlc3QoKTtcclxuXHJcbmV4cG9ydCB7IFN0b2NrLCBTdG9ja01vYmlsZSB9O1xyXG4iXSwic291cmNlUm9vdCI6IiJ9