# Tcp-socket-programming-csharp
TCP multi-client socket programming project in C# with role-based access and file operations

## Group
Group 3 – TCP Protocol

## Description
This project implements a TCP server capable of handling multiple clients concurrently. Clients can connect, send commands, and interact with files stored on the server. The system includes role-based permissions where an admin has full access (read, write, execute) while other users have read-only access.

## Features
- Multi-client support (thread-based)
- TCP socket communication
- Role-based authentication
- File operations (read, write, delete)
- Command-based interaction

## Commands
- MSG <text>
- LIST
- READ <file>
- WRITE <file> <content> (admin only)
- DELETE <file> (admin only)
- EXECUTE (admin only)
- EXIT

## Project Structure
ClientApp/     → Client implementation  
ServerApp/     → Server implementation  
server_files/  → File storage (server side)

## Team
- Albina Haliti
- Alba Jashanica
- Albion Hoxha
- Aid Haxhimeri

## Running
1. Set IP and PORT in both client and server
2. Run server
3. Run at least 4 clients (same network)

## Purpose
To demonstrate TCP socket programming, multi-client handling, and controlled file access in C#.