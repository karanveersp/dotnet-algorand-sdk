using Algorand;
using Algorand.Client;
using Algorand.V2;
using System;

namespace sdk_examples.V2.contract
{
    class LogicSignature
    {
        public static void Main(params string[] args)
        {
            string ALGOD_API_ADDR = args[0];
            if (ALGOD_API_ADDR.IndexOf("//") == -1)
            {
                ALGOD_API_ADDR = "http://" + ALGOD_API_ADDR;
            }
            string ALGOD_API_TOKEN = args[1];
            //��һ���˺����ڸ����ܺ�Լǩ��������ǩ��������ȥ
            string SRC_ACCOUNT = "typical permit hurdle hat song detail cattle merge oxygen crowd arctic cargo smooth fly rice vacuum lounge yard frown predict west wife latin absent cup";
            Account acct1 = new Account(SRC_ACCOUNT);            
            // format and send logic sig
            byte[] program = Convert.FromBase64String("ASABASI=");

            LogicsigSignature lsig = new LogicsigSignature(program, null);            

            // sign the logic signaure with an account sk
            acct1.SignLogicsig(lsig);
            var contractSig = Convert.ToBase64String(lsig.sig.Bytes);
            var acct1Address = acct1.Address.ToString();

            //�ڶ�������һ���˺ţ�ֻ��contractSig�Ϳ��Զ�acct1�н��з������ܺ�Լ�߼��Ĳ���
            //ע����������ȫ������1,Ҳ����˵�κβ���������ʵ���������Ƿǳ�Σ�յĲ�������ע��
            //ע��Ҳ��Ҫ�õ�acct1�ĵ�ַ����Կ��

            //ʵ���ϣ��ڱ����в�����Ҫacct2��˽Կ��ֻҪ�й�Կ���㹻��
            //string acct2_mnemonic = "place blouse sad pigeon wing warrior wild script"
            //                   + " problem team blouse camp soldier breeze twist mother"
            //                   + " vanish public glass code arrow execute convince ability"
            //                   + " there";
            //Account acct2 = new Account(acct2_mnemonic);
            var acct2Address = "AJNNFQN7DSR7QEY766V7JDG35OPM53ZSNF7CU264AWOOUGSZBMLMSKCRIU";

            //Ϊ�˱�ʾ���˺�1ȫ�����룬�����½�һ��LogicsigSignature
            LogicsigSignature lsig2 = new LogicsigSignature(program, null, Convert.FromBase64String(contractSig));
            var algodApiInstance = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);
            Algorand.V2.Model.TransactionParametersResponse transParams;
            try
            {
                transParams = algodApiInstance.TransactionParams();
            }
            catch (ApiException e)
            {
                throw new Exception("Could not get params", e);
            }

            Transaction tx = Utils.GetPaymentTransaction(new Address(acct1Address), new Address(acct2Address), 1000000, 
                "draw algo with logic signature", transParams);            
            
            try
            {
                //bypass verify for non-lsig
                SignedTransaction signedTx = Account.SignLogicsigTransaction(lsig2, tx);
                var id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Successfully sent tx logic sig tx id: " + id);
            }
            catch (ApiException e)
            {
                // This is generally expected, but should give us an informative error message.
                Console.WriteLine("Exception when calling algod#rawTransaction: " + e.Message);
            }
            
            Console.WriteLine("You have successefully arrived the end of this test, please press and key to exist.");
            Console.ReadKey();
        }
    }
}