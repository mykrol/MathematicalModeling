function InitHomeController(Problem)
{
    var app = angular.module("MathModApp");

    app.filter('range', function () {
        return function (input, total) {
            total = parseInt(total);
            for (var i = 0; i < total; i++)
                input.push(i);
            return input;
        };
    });
    app.controller("HomeController", function ($scope) {

        $scope.GrinFunction = Problem.GrinFunction;
        $scope.TValue = Problem.TValue;
        $scope.XL = [];
        $scope.Y0L = [];
        $scope.SL = [];
        $scope.YHL=[];

        $scope.addStartCondRow = function (i) {
            if (i == $scope.StartCondTableRowsNumber - 1)
                $scope.StartCondTableRowsNumber++;
        }
        $scope.StartCondTableRowsNumber = 6;
        $scope.BorderCondTableRowsNumber = 6;
        $scope.addBorderCondRow = function (i) {
            if (i == $scope.BorderCondTableRowsNumber - 1)
                $scope.BorderCondTableRowsNumber++;
        }


    });
}