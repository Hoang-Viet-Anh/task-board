# Stage 1: Build the Angular app
FROM node:20-bullseye AS build

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm ci --legacy-peer-deps

COPY . .

RUN npm run build -- --configuration=production

RUN ls -la /app/dist/frontend/browser

# Stage 2: Serve the app with Nginx
FROM nginx:alpine

COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html

RUN cd /usr/share/nginx/html && if [ -f index.csr.html ]; then mv index.csr.html index.html; fi

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 4200

CMD ["nginx", "-g", "daemon off;"]