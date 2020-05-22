FROM nginx:alpine

COPY default.conf /etc/nginx/conf.d/
COPY index.html /var/www/
COPY assets/js/* /var/www/js/
COPY assets/css/* /var/www/css/

EXPOSE 80