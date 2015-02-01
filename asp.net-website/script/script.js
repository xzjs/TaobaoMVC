// JavaScript Document
var basePath = "http://localhost:57445/";
var taobaoMVC = angular.module('taobaoMVC',['ngCookies']);
taobaoMVC.controller("ProductCategory", ["$scope", "$http", function ($scope, $http) {
	// 获得产品分类列表
	$http({method:"GET", url: basePath + "ProductCategory/"}).success(function (data) {
		$scope.categorys = data;
	}).error(function (data, status) {
		//
	});
}]);
taobaoMVC.controller("ProductList", ["$scope", "$http", function ($scope, $http) {
	// 获得产品列表，按分类区分
	$http({method:"GET", url: basePath + "Home/"}).success(function (data) {
		$scope.products = data;
	}).error(function (data, status) {
		//
	});
}]);
taobaoMVC.controller("ProductDetail", ["$scope", "$http", function ($scope, $http) {
	// 获得产品详细信息
	var queryString = location.search.length > 0 ? location.search.substring(1) : "";
	if (queryString.length == 0) {
		window.location.replace("index.html");
	}
	var query = decodeURIComponent(queryString).split("=");
	if (query[0] === "id") {
		$http({method:"GET", url: basePath + "Home/ProductDetail/" + query[1]}).success(function (data) {
			$scope.product = data;
			console.log(data);
		}).error(function (data, status) {
			//
		});
	} else{
		// 跳转回首页
		window.location.replace("index.html");
	}
	
}]);
taobaoMVC.controller("ListShow", ["$scope", "$http", function ($scope, $http) {
	// 查看单独一个分类的产品或展示搜索结果
	var queryString = location.search.length > 0 ? location.search.substring(1) : "";
	if (queryString.length == 0) {
		window.location.replace("index.html");
	}
	var query = decodeURIComponent(queryString).split("=");
	if (query[0] === "categoryId") {
		// 查看单独一个分类的产品
		$http({method:"GET", url: basePath + "Home/index/" + query[1]}).success(function (data) {
			$scope.titleString = data[0].name || "";
			$scope.products = data[0].products || {};
		}).error(function (data, status) {
			//
		});
	} else if(query[0] === "search") {
		// 展示搜索结果
		$http({method:"POST", url: basePath + "Home/Search/", data: "str=" + query[1], headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data) {
			$scope.titleString = "搜索“" + query[1] + "”的结果：";
			if (data.length == 0) {
				$scope.tips = "无相关信息！";
				return;
			}
			$scope.products = data;
		}).error(function (data, status) {
			$scope.titleString = "搜索失败..."
		});
	} else {
		// 跳转回首页
		window.location.replace("index.html");
	}
}]);
taobaoMVC.controller("RegUser", ["$scope", "$http", function ($scope, $http) {
	// 用户注册
	$scope.tips = "注册",
	$scope.user = {};
	$scope.regUser = function () {
		if ($scope.user.Password === $scope.user.PasswordAgain) {
			$scope.tips = "正在注册中，请稍后...";
			delete $scope.user.PasswordAgain;
			$http({method:"POST", url: basePath + "Member/Register/", data: $.param($scope.user), headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
				$scope.tips = data;
			}).error(function (data, status) {
				$scope.tips = "注册失败";
			});
		} else{
			alert("两次密码输入应一致！");
			delete $scope.user.PasswordAgain;
		}
	}
}]);
taobaoMVC.controller("LoginUser", ["$scope", "$http", "$cookieStore", function ($scope, $http, $cookieStore) {
	// 用户登录
	$scope.tips = "登录",
	$scope.user = {};
	$scope.login = function () {
		$scope.tips = "正在登录中，请稍后...";
		$http({method:"POST", url: basePath + "Member/Login/", data: $.param($scope.user), headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
			if (data.token) {
				console.log(data.token);
				$cookieStore.put("token", data.token);
				$scope.tips = "登录成功";
				setTimeout(function () {
					if (data.IsAdmin) {
						location.href = "manage.html";
					} else{
//						location.href = "index.html";
					}
				},1000);
			} else{
				$scope.tips = data;
			}
		}).error(function (data, status) {
			$scope.tips = "登录失败";
		});
	}
}]);
taobaoMVC.controller("UserStatus", ["$scope", "$http", "$cookieStore", function ($scope, $http, $cookieStore) {
	// 验证用户身份
	var token = $cookieStore.get("token");
	if (token) {
		$scope.tips = "正在自动登录...";
		$http({method:"POST", url: basePath + "Member/GetMember/", data: "token=" + token, headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
			if (data.name) {
				$scope.tips = "[" + data.name + "]";
				$scope.target = "order.html";
				$scope.logoutText = "[注销]";
			} else{
				$scope.tips = "验证身份失败";
				alert(data);
			}
		}).error(function (data, status) {
			$scope.tips = "登录失败";
		});
		$scope.logout = function () {
			$http({method:"POST", url: basePath + "Member/Logout/", data: "token=" + token, headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
				console.log(data);
				if (data) {
					$cookieStore.remove("token");
					location.reload();
				}
			}).error(function (data, status) {
				$scope.logoutText = "注销失败";
			});
		}
	} else{
		$scope.tips = "请[登录]或者[免费注册]";
		$scope.target = "loginOrReg.html";
	}
	
}]);
taobaoMVC.controller("CartList", ["$scope", "$http", function ($scope, $http) {
	// 购物车列表
//	$http({method:"GET", url: basePath + "ProductCategory/"}).success(function (data) {
//		$scope.categorys = data;
//	}).error(function (data, status) {
//		//
//	});
}]);
taobaoMVC.controller("ManageController", ["$scope", "$http", "$cookieStore", function ($scope, $http, $cookieStore) {
	// 后台管理身份验证
	var token = $cookieStore.get("token"),
		editData = {
			"token": token,
			"Name": $scope.categoryName
		};
	if (!token) {
		window.location.replace("loginOrReg.html");
	}
	$http({method:"POST", url: basePath + "Member/GetMember/", data: "token=" + token, headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
		if (!data.IsAdmin) {
			// 不是管理员
			window.location.replace("loginOrReg.html");
		}
		$scope.tips = "欢迎回来，管理员。";
		$scope.logoutText = "[注销]";
	}).error(function (data, status) {
		window.location.replace("loginOrReg.html");
	});
	$scope.logout = function () {
		$http({method:"POST", url: basePath + "Member/Logout/", data: "token=" + token, headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
			if (data) {
				$cookieStore.remove("token");
				location.reload();
			}
		}).error(function (data, status) {
			$scope.logoutText = "注销失败";
		});
	}
}]);
taobaoMVC.controller("EditCategory", ["$scope", "$http", "$cookieStore", function ($scope, $http, $cookieStore) {
	// 后台分类编辑，删除
	var token = $cookieStore.get("token");
	$scope.editFormShow = false;
	$scope.deleteFormShow = false;
	$scope.editToggle = function () {
		$scope.editFormShow = !$scope.editFormShow;
	}
	$scope.deleteToggle = function () {
		$scope.deleteFormShow = !$scope.deleteFormShow;
	}
	$scope.editCategory = function (e) {
		$http({method:"POST", url: basePath + "ProductCategory/Edit/" + e.id, data: "token=" + token + "&Name=" + $scope.categoryName, headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
			if (data === true) {
				alert("修改成功！");
				location.reload();
			} else {
				alert(data);
			}
		}).error(function (data, status) {
			alert("网络出错！");
		});
	}
	$scope.deleteCategory = function (e) {
		alert("删除分类！");
		console.log(e.id)
		$http({method:"POST", url: basePath + "ProductCategory/Delete/" + e.id, data: "token=" + token + "&id=" + e.id, headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
			console.log(data);
			if (data === true) {
				alert("删除成功！");
				location.reload();
			} else {
				alert(data);
			}
		}).error(function (data, status) {
			alert("网络出错！");
		});
	}
}]);
taobaoMVC.controller("CreateCategory", ["$scope", "$http", "$cookieStore", function ($scope, $http, $cookieStore) {
	// 后台分类添加
	var token = $cookieStore.get("token");
	$scope.createFormShow = false;
	$scope.createToggle = function () {
		$scope.createFormShow = !$scope.createFormShow;
	}
	$scope.createCategory = function (e) {
		$http({method:"POST", url: basePath + "ProductCategory/Create/", data: "token=" + token + "&Name=" + $scope.categoryName, headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
			if (data === true) {
				alert("添加成功！");
				location.reload();
			} else {
				alert(data);
			}
		}).error(function (data, status) {
			alert("网络出错！");
		});
	}
}]);
taobaoMVC.controller("CreateProduct", ["$scope", "$http", "$cookieStore", function ($scope, $http, $cookieStore) {
	// 后台分类添加
	var token = $cookieStore.get("token");
	$scope.createFormShow = false;
	$scope.createToggle = function () {
		$scope.createFormShow = !$scope.createFormShow;
	}
	$scope.createProduct = function (e) {
		$http({method:"POST", url: basePath + "ProductCategory/Create/", data: "token=" + token + "&Name=" + $scope.categoryName, headers: {'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'}}).success(function (data, status) {
			if (data === true) {
				alert("添加成功！");
				location.reload();
			} else {
				alert(data);
			}
		}).error(function (data, status) {
			alert("网络出错！");
		});
	}
}]);
(function() {
	$.extend({
		notice: function(notice) {
			var status_timer;  //设置计时器
			$(notice).hover(function(){
				window.clearTimeout(status_timer);
			},function () {
				status_timer=window.setTimeout(status_scroll,5000);
			});
			status_timer=window.setTimeout(status_scroll,1000);
			
			function status_scroll(){
				$(notice).children("ul").animate({top:"-40px"},1000,function(){
					$(notice).find("li:first").appendTo(notice + " ul");
					$(notice).children("ul").css("top","0");
				});
				status_timer=window.setTimeout(status_scroll,5000);//轮换速度
			}
		},
		regInput: function(regInput) {
			var $in = $(regInput).children("input,textarea");
			$in.focus(function() {
				$(this).siblings("span").hide();
			})
			$in.focusout(function() {
				if (!$(this).val()) {
					$(this).siblings("span").show();
				}
			})
			$in.each(function() {
				if ($(this).val()) {
					$(this).siblings("span").hide();
				}
			})
		},
		changeInfo: function(btn) {
			var $btn = $(btn);
			$btn.on("click", function() {
				$(this).prev().removeAttr("disabled").parents(".info").siblings("input").show();
			})
		},
		scrollFixed: function (scrollFixedElement) {
			var $scrollFixed = $(scrollFixedElement);
			var top = $scrollFixed.offset().top;
			$(window).scroll(function() {
				if ($(window).scrollTop() > top) {
					$scrollFixed.addClass("fixed");
				} else if ( ($(window).scrollTop() < top) && ($scrollFixed.css("position") == "fixed") ) {
					$scrollFixed.removeClass("fixed");
				}
			});
		}
	});
})(jQuery);

$(document).ready(function() {
	var $tag = $(".reg");
	if (!!$tag.html()) {
		$tag.find("li a").on("click", function() {
			$(this).addClass("active").parent().siblings().children("a").removeClass("active");
			$($tag.find(".reg-con").get($(this).parent().index())).css("display", "block").siblings(".reg-con").hide();
			return false;
		})
	}
});
