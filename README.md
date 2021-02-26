# FilmRentals

Project for testing various configurations of a Web API hosted by Kubernetes, running against MySQL and Azure Database for MySQL:

- **mysqlapp.yml**: Basic Kubernetes config for Web API only
- **mysql-deployment.yml**: Kubernetes config for Web API + containerized MySQL
- **mysqlserver.yml** + **mysqlazure-deployment.yml**: Kubernetes configs for deploying Azure Database for MySQL using [Azure Service Operator](https://operatorhub.io/operator/azure-service-operator) and Web API that connects using secrets.
