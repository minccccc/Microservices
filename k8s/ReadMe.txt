#Create secret for MSSQL server
kubectl create secret generic mssql --from-literal=SA_PASSWORD="pa55w0rd!"

#Add migrations
dotnet ef migrations add InitialMigration