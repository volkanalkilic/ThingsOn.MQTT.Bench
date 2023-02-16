docker stop thingson-mqtt-bench
docker rm thingson-mqtt-bench
docker rmi thingson-mqtt-bench

docker build -t "thingson-mqtt-bench" -f Dockerfile .

docker run -it --env-file ./.env --name "thingson-mqtt-bench" "thingson-mqtt-bench"
