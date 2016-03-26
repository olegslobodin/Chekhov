angular.module('homeCtrl', [])
    .controller('HomeController', function ($scope, $http) {

        getProfileInfo = function () {
            $scope.myHometown = "loading info...";
            $http({
                method: 'GET',
                url: "/api/Me",
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                }
            }).then(function mySucces(response) {
                $scope.myHometown = 'Ваше место рождения: ' + response.data.hometown;
            }, function myError(response) {
                $scope.myHometown = "Error";
            });
        };

        getMyCompositions = function () {
            $http({
                method: 'GET',
                url: "/odata/Compositions",
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                }
            }).then(function mySucces(response) {
                $scope.myCompositions = response.data.value;
            });
        }

        initializeHome = function () {
            getProfileInfo();
            getMyCompositions();
        }


        $scope.postComposition = function () {
            $scope.saving = true;

            $http({
                method: 'POST',
                url: "/odata/Compositions",
                data: JSON.stringify({ 'Id': '0', 'UserId': '', 'Name': $scope.draftName, 'Text': $scope.draft }),
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken(),
                    'contentType': "application/json; charset=utf-8"
                }
            }).then(function mySucces(response) {
                $scope.saving = false;
                getMyCompositions();
            }, function myError(response) {
                $scope.saving = false;
            });

            $scope.draft = '';
            $scope.draftName = '';
        };

        initializeHome();
    });