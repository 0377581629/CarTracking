echo ------------- Entering directory ----------
cd payment_demo

echo ------------- Compiling ---------------
dotnet publish -c Release -o /home/audioplus/domains/audioplus.vn/public_html/alepay

echo ------------- Creating needed directories -------------
mkdir -p -v /home/audioplus/domains/audioplus.vn/public_html/alepay/logs
mkdir -p -v /home/audioplus/domains/audioplus.vn/public_html/alepay/webhooks

echo Done. Goodbye!
