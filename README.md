# ApiUsuariosMVC

API REST MVC de Usuarios, con securización JWT. Utiliza EF para la gestión de usuarios en una BBDD SQL Server.

Script para creación de BBDD y primer usuario: (para este ejemplo, no se han encriptado las contraseñas, lo ideal sería hacerlo con SHA256)
___________________________________________________
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Login NVARCHAR (50) NOT NULL,
    Pwd NVARCHAR (50) NOT NULL,
    Balance money,
    Administrador bit NOT NULL,
);

INSERT INTO USUARIOS(Login,Pwd,Balance,Administrador) values('admin','admin101','1000.90',1)
___________________________________________________


A los métodos de /admin sólo podrán acceder los usuarios con rol Administrador, a los métodos de /user podrán acceder todos los usuarios previamente logados.

Estos métodos están documentados en Swagger.

Para hacer pruebas, lo primero que hay que hacer es llamar al método api/login/authenticate, que devolverá un token.

En el recuadro superior derecho del swagger, hay que escribir 'Bearer (token)' donde token es el token que hemos recibido del método anterior, y darle al botón 'Explore'.

Si no hacemos esto, los demás métodos nos devolverán un 401 y un msensaje.

Las respuestas en la mayoría de los casos de gestionan con unas clases en la carpeta 'Helpers' diseñadas para ello, que devuelven un HttpResponseMessage para tener todo 
más estructurado e intuitivo.
