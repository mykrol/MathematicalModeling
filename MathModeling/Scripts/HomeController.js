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
        $scope.x1Start = 0;
        $scope.x1End = 4;
        $scope.x2Start = 0;
        $scope.x2End = 3;
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


        $scope.u = function(x1,x2,t)
        {
            return 10*Math.sin(t * x1) * Math.sin(x2 * t) + 10;
        }


        $scope.DrawPlot = function(t)
        {
            var N = 20;
            var h1 = ($scope.x1End - $scope.x1Start) / N;
            var h2 = ($scope.x2End - $scope.x2Start) / N;

            var x1 = $scope.x1Start;
            var x2 = $scope.x2Start;

            var a  =[];
            var b=[];
            var c = [];

            while (x1 < $scope.x1End && x2 < $scope.x2End)
            {
                //a.push(x1);
                //b.push(x2);
                //c.push($scope.u(x1, x2, t));
                var a_ = x1;
                var b_ = x2;
                var c_ = $scope.u(x1, x2, t);
                //var a_ = Math.random();
                a.push(a_);

                //var b_ = Math.random();
                b.push(b_);

                //var c_ = Math.random();
                c.push(c_);
                x1+=h1;
                x2+=h2;
            }
            var data=[
                        {
                opacity:0.8,
                color:'rgb(300,100,200)',
                type: 'scatter3d',
                x: a,
                y: b,
                z: c,
                }
            ];
            var layout = {
                autosize: false,
                width: 500,
                height: 500
            };
            Plotly.newPlot('plot', data, layout);
        }

    });
}