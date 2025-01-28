REM since i have slightly altered the calls, i have made my own curl data.
echo 1) this will create a user:
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"something\",    \"Password\":\"password\"}"

echo 2) this will login a user:
curl -i -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"something\",    \"Password\":\"password\"}"

echo 3) this will show all packages that you can buy:
curl -i -X POST http://localhost:10001/showpackages --header "Content-Type: application/json" -d ""

echo 4) this will buy a Package:
curl -i -X POST http://localhost:10001/package --header "Content-Type: application/json" -d "{\"id\":2, \"token\":\"something-mtcgToken\"}"

echo 5) this will get the 10 best players by elo
curl -i -X POST http://localhost:10001/score --header "Content-Type: application/json" -d ""

echo 6)this will change the username
curl -i -X POST http://localhost:10001/change --header "Content-Type: application/json" -d "{\"name\":\"newsomething\", \"token\":\"something-mtcgToken\"}"
echo 6)this will change the password
curl -i -X POST http://localhost:10001/change --header "Content-Type: application/json" -d "{\"pw\":\"newpassword\", \"token\":\"something-mtcgToken\"}"

echo 7) this will edit your current deck
curl -i -X POST http://localhost:10001/editdeck --header "Content-Type: application/json" -d "{\"token\":\"something-mtcgToken\", \"cards\":[1, 2, 3] "}"

echo 8) this will show your current deck:
curl -i -X POST http://localhost:10001/showdeck --header "Content-Type: application/json" -d "{\"token\":\"something-mtcgToken\"}"

echo 9) this will show the players stats:
curl -i -X POST http://localhost:10001/profile --header "Content-Type: application/json" -d "{\"token\":\"something-mtcgToken\"}"

this will start the queueing process for a player:
curl -i -X POST http://localhost:10001/queue --header "Content-Type: application/json" -d "{\"token\":\"something-mtcgToken\"}"