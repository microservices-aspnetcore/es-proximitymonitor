curl -H "Content-Type: application/json" -X POST -d '{"ID":"2224e9b4-c3e5-41f3-a640-185a6f116276","Name":"Red Team"}' http://localhost:5001/teams
curl -H "Content-Type: application/json" -X POST -d '{"ID":"696aab3b-dd12-458b-bfb5-54e41f234d65","firstName":"Red","lastName": "Leader"}' http://localhost:5001/teams/2224e9b4-c3e5-41f3-a640-185a6f116276/members
curl -H "Content-Type: application/json" -X POST -d '{"ID":"a0c2afd9-88d9-4f38-a5cc-c30977717ea1","firstName":"Red","lastName": "Soldier"}' http://localhost:5001/teams/2224e9b4-c3e5-41f3-a640-185a6f116276/members
