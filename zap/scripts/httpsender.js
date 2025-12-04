var LogManager = Java.type('org.apache.logging.log4j.LogManager');
var logger = LogManager.getLogger('httpsender.js');

function sendingRequest(msg, initiator, helper) {

  logger.info("[HTTPSENDER] \\/ --------------------- SENDING REQUEST --------------------- \\/");

  var url = msg.getRequestHeader().getURI().toString();
  var method = msg.getRequestHeader().getMethod();
  
  logger.info("[HTTPSENDER] Sending " + method + " request to: " + url);
  
  // Check if Authorization header is present
  var authHeader = msg.getRequestHeader().getHeader("Authorization");
  if (authHeader) {
    logger.info("[HTTPSENDER] Authorization header present: " + authHeader.substring(0, 20) + "...");
  } else {
    logger.info("[HTTPSENDER] No Authorization header");
  }

  logger.info("[HTTPSENDER] /\\ --------------------- SENDING REQUEST --------------------- /\\");
}

function responseReceived(msg, initiator, helper) {

  logger.info("[HTTPSENDER] \\/ -------------------- RESPONSE RECEIVED -------------------- \\/");

  var url = msg.getRequestHeader().getURI().toString();
  var statusCode = msg.getResponseHeader().getStatusCode();
  var responseBodyLength = msg.getResponseBody().length();
  
  logger.info("[HTTPSENDER] Response received from: " + url);
  logger.info("[HTTPSENDER] Status: " + statusCode + ", Body length: " + responseBodyLength + " bytes");
  
  // Log if this is the auth endpoint
  if (url.toLowerCase().indexOf("/authorisation") !== -1) {
    logger.info("[HTTPSENDER] Auth endpoint response detected");
  }

  logger.info("[HTTPSENDER] /\\ -------------------- RESPONSE RECEIVED -------------------- /\\");
}

function getRequiredParamsNames() {
  return [];
}

function getOptionalParamsNames() {
  return [];
}
