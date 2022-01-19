docker build -t registry.heroku.com/freebirdcore/web .
docker push registry.heroku.com/freebirdcore/web
heroku container:release web --app freebirdcore