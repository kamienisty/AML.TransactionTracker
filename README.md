In Docker folder there's docker-build-and-run.bat for easy setting up application in containers.

From Docker it should be available as https://localhost:8081/swagger

RabbitMQ can be accessed at http://localhost:15672/ with standard login and password (L: guest, P: guest)

Application uses SQLite as database. File for it is located in database folder. When application is running in container an I/O exception can occur in case database file is being opened by other program (like DB Browser). I'd recommend closing any such programs before lunching application. 
