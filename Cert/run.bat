openssl genpkey -outform PEM -algorithm RSA -pkeyopt rsa_keygen_bits:4096 -out OfflineKey.key
openssl genpkey -outform PEM -algorithm RSA -pkeyopt rsa_keygen_bits:2048 -out global.key
openssl genpkey -outform PEM -algorithm RSA -pkeyopt rsa_keygen_bits:1024 -out AppTicket.key
openssl req -new -nodes -key global.key -config global.conf -nameopt utf8 -utf8 -out global.csr
openssl req -x509 -nodes -in global.csr -days 3650 -key global.key -config global.conf -extensions req_ext -nameopt utf8 -utf8 -out global.crt
certutil -p "global,global" -mergepfx global.crt global.pfx
pause