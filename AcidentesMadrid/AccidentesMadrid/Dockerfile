# ============================================================
# Dockerfile multi-etapa: Accidentes de Madrid
# ============================================================


# ------------------------------------------------------------
# ETAPA 1: Build + Publish
# ------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src


# Copiamos todo el contenido del proyecto
COPY . .


# Rider usa como contexto la carpeta superior,
# por eso entramos en la carpeta donde está el .csproj.
WORKDIR /src/AccidentesMadrid


# Eliminamos posibles compilaciones anteriores de Windows.
RUN rm -rf bin obj


# Restauramos las dependencias del proyecto dentro de Linux.
RUN dotnet restore AccidentesMadrid.csproj --force


# Publicamos la aplicación en modo Release.
RUN dotnet publish AccidentesMadrid.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish



# ------------------------------------------------------------
# ETAPA 2: Runtime
# ------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime

WORKDIR /app


# Copiamos la aplicación ya publicada.
COPY --from=build /app/publish .


# Copiamos los CSV utilizados por las consultas.
COPY --from=build /src/AccidentesMadrid/Data ./Data


# Ejecutamos la aplicación de consola.
ENTRYPOINT ["dotnet", "AccidentesMadrid.dll"]