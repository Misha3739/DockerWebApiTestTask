set -e
run_cmd="dotnet run --server.urls http://*:8060"

until dotnet ef database update; do
>&2 echo "Db Server is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
exec $run_cmd